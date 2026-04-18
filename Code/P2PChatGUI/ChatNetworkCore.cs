using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace P2PChatGUI.Core
{
    public class ChatNetworkCore : IAsyncDisposable, IDisposable
    {
        private TcpListener? _listener;
        private TcpClient? _client;
        private NetworkStream? _stream;

        private CancellationTokenSource _cts = new();
        private Task? _listenTask;

        // Flag để tránh gọi OnDisconnected nhiều lần
        private volatile bool _isDisconnected = false;

        // Giới hạn kích thước tin nhắn (10MB) để tránh tấn công
        private const int MaxMessageSize = 10 * 1024 * 1024;

        public event Action<string>? OnMessageReceived;
        public event Action<string>? OnSystemMessage;
        public event Action? OnDisconnected;

        /// <summary>
        /// Bắt đầu làm Host (lắng nghe kết nối).
        /// </summary>
        public async Task<bool> StartListeningAsync(string ipAddress, string portString)
        {
            if (!IsValidIpAddress(ipAddress))
            {
                OnSystemMessage?.Invoke("IP không hợp lệ.");
                return false;
            }

            if (!IsValidPort(portString, out int port))
            {
                OnSystemMessage?.Invoke("Port không hợp lệ. Port phải là số từ 1 đến 65535.");
                return false;
            }

            try
            {
                IPAddress ip = IPAddress.Parse(ipAddress.Trim());
                _listener = new TcpListener(ip, port);
                _listener.Start();
                OnSystemMessage?.Invoke($"Đang chờ đối phương kết nối tại {ip}:{port}...");

                while (true)
                {
                    _client = await _listener.AcceptTcpClientAsync(_cts.Token);
                    if (_client.Client.RemoteEndPoint != null &&
                        _client.Client.LocalEndPoint != null &&
                        _client.Client.RemoteEndPoint.Equals(_client.Client.LocalEndPoint))
                    {
                        _client.Close();
                        continue;
                    }
                    break;
                }

                ConfigureClient(_client);

                _stream = _client.GetStream();

                OnSystemMessage?.Invoke("Đã có người kết nối tới!");
                _listener.Stop(); // Chỉ hỗ trợ 1-1

                _listenTask = ListenForMessagesAsync(_cts.Token);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception ex)
            {
                OnSystemMessage?.Invoke($"Lỗi tạo phòng: {ex.Message}");
                await CleanupAsync();
                return false;
            }
        }

        /// <summary>
        /// Kết nối đến peer (làm Client).
        /// </summary>
        public async Task<bool> ConnectAsync(string ipAddress, string portString)
        {
            if (!IsValidIpAddress(ipAddress))
            {
                OnSystemMessage?.Invoke("IP không hợp lệ.");
                return false;
            }

            if (!IsValidPort(portString, out int port))
            {
                OnSystemMessage?.Invoke("Port không hợp lệ. Port phải là số từ 1 đến 65535.");
                return false;
            }

            try
            {
                _client = new TcpClient();
                ConfigureClient(_client);

                await _client.ConnectAsync(ipAddress, port, _cts.Token);
                _stream = _client.GetStream();

                OnSystemMessage?.Invoke("Đã kết nối thành công!");

                _listenTask = ListenForMessagesAsync(_cts.Token);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            catch (Exception ex)
            {
                OnSystemMessage?.Invoke($"Lỗi kết nối: {ex.Message}");
                await CleanupAsync();
                return false;
            }
        }

        private static bool IsValidPort(int port) => port >= 1 && port <= 65535;

        public static bool IsValidPort(string portString, out int port)
        {
            if (!string.IsNullOrWhiteSpace(portString) && int.TryParse(portString.Trim(), out port))
            {
                return port >= 1 && port <= 65535;
            }

            port = 0;
            return false;
        }

        private static bool IsValidIpAddress(string ipAddress)
        {
            return !string.IsNullOrWhiteSpace(ipAddress) &&
                   IPAddress.TryParse(ipAddress.Trim(), out _);
        }

        private void ConfigureClient(TcpClient client)
        {
            client.NoDelay = true;                    // Gửi tin nhắn ngay lập tức (thấp latency)
            client.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
            // Có thể thêm TcpKeepAliveTime, TcpKeepAliveInterval nếu cần (chỉ hỗ trợ một số nền tảng)
        }

        /// <summary>
        /// Gửi tin nhắn với Length-Prefix (4 byte).
        /// </summary>
        public async Task SendMessageAsync(string message)
        {
            if (_stream == null || !_client?.Connected == true || string.IsNullOrWhiteSpace(message))
                return;

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                if (data.Length > MaxMessageSize)
                {
                    OnSystemMessage?.Invoke("Tin nhắn quá dài!");
                    return;
                }

                byte[] lengthPrefix = BitConverter.GetBytes(data.Length);

                await _stream.WriteAsync(lengthPrefix, _cts.Token);
                await _stream.WriteAsync(data, _cts.Token);
                await _stream.FlushAsync(_cts.Token);   // Đảm bảo gửi ngay
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                OnSystemMessage?.Invoke("Lỗi khi gửi tin nhắn.");
                await HandleDisconnectionAsync();
            }
        }

        /// <summary>
        /// Vòng lặp nhận tin nhắn với Length-Prefix.
        /// </summary>
        private async Task ListenForMessagesAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested && _stream != null)
                {
                    // Đọc 4 byte length
                    byte[] lengthBuffer = new byte[4];
                    int bytesRead = await ReadExactlyAsync(_stream, lengthBuffer, 4, token);

                    if (bytesRead == 0)
                    {
                        await HandleDisconnectionAsync();
                        return;
                    }

                    int dataSize = BitConverter.ToInt32(lengthBuffer, 0);

                    if (dataSize <= 0 || dataSize > MaxMessageSize)
                    {
                        OnSystemMessage?.Invoke("Nhận được tin nhắn không hợp lệ (kích thước sai).");
                        await HandleDisconnectionAsync();
                        return;
                    }

                    // Đọc nội dung tin nhắn
                    byte[] dataBuffer = new byte[dataSize];
                    bytesRead = await ReadExactlyAsync(_stream, dataBuffer, dataSize, token);

                    if (bytesRead == 0)
                    {
                        await HandleDisconnectionAsync();
                        return;
                    }

                    string message = Encoding.UTF8.GetString(dataBuffer);
                    OnMessageReceived?.Invoke(message);
                }
            }
            catch (OperationCanceledException)
            {
                // Shutdown bình thường
            }
            catch (Exception ex)
            {
                if (!_isDisconnected)
                {
                    OnSystemMessage?.Invoke($"Lỗi nhận tin nhắn: {ex.Message}");
                }
                await HandleDisconnectionAsync();
            }
        }

        /// <summary>
        /// Helper: Đọc chính xác số byte yêu cầu (hoặc 0 nếu disconnect).
        /// </summary>
        private static async Task<int> ReadExactlyAsync(NetworkStream stream, byte[] buffer, int count, CancellationToken token)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int read = await stream.ReadAsync(buffer, totalRead, count - totalRead, token);
                if (read == 0) 
                    return 0; // Đối phương ngắt kết nối

                totalRead += read;
            }
            return totalRead;
        }

        private async Task HandleDisconnectionAsync()
        {
            if (_isDisconnected) return;
            _isDisconnected = true;

            OnSystemMessage?.Invoke("Người dùng kia đã thoát hoặc kết nối bị ngắt.");
            OnDisconnected?.Invoke();

            await CleanupAsync();
        }

        private async Task CleanupAsync()
        {
            try
            {
                _cts.Cancel();

                if (_stream != null)
                {
                    await _stream.DisposeAsync();
                    _stream = null;
                }

                _client?.Close();
                _client?.Dispose();
                _client = null;

                _listener?.Stop();
                _listener = null;

                _cts.Dispose();
                _cts = new CancellationTokenSource();
            }
            catch { /* ignore cleanup errors */ }
        }

        /// <summary>
        /// Ngắt kết nối sạch sẽ.
        /// </summary>
        public async Task DisconnectAsync()
        {
            await CleanupAsync();

            // Chờ task lắng nghe kết thúc (tối đa 2 giây)
            if (_listenTask != null)
            {
                try
                {
                    await Task.WhenAny(_listenTask, Task.Delay(2000));
                }
                catch { }
            }
        }

        public void Disconnect() => DisconnectAsync().Wait(1000); // Phiên bản sync cho tiện

        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync();
            _cts.Dispose();
        }

        public void Dispose()
        {
            Disconnect();
            _cts.Dispose();
        }
    }
}