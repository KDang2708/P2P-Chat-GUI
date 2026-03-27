using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Chatter.Core
{
    /// <summary>
    /// Lớp xử lý lõi mạng cho ứng dụng chat, cung cấp các chức năng kết nối, gửi và nhận tin nhắn qua TCP.
    /// </summary>
    public class ChatNetworkCore
    {
        // Server lắng nghe kết nối đến
        private TcpListener _listener;
        // Client TCP dùng để kết nối đến peer
        private TcpClient _client;
        // Luồng dữ liệu mạng để thực hiện thao tác đọc/ghi
        private NetworkStream _stream;

        // Sự kiện phát ra khi nhận được tin nhắn mới
        public event Action<string> OnMessageReceived;
        // Sự kiện phát ra cho các thông báo hệ thống (như thông báo lỗi, trạng thái kết nối)
        public event Action<string> OnSystemMessage;
        // Sự kiện phát ra khi bị ngắt kết nối
        public event Action OnDisconnected;

        /// <summary>
        /// Bắt đầu phân hệ lắng nghe (Host) cho một cổng cụ thể.
        /// </summary>
        /// <param name="port">Cổng mạng để mở lên lắng nghe</param>
        public async Task StartListeningAsync(int port)
        {
            try
            {
                _listener = new TcpListener(System.Net.IPAddress.Any, port);
                _listener.Start();
                OnSystemMessage?.Invoke($"Đang chờ đối phương kết nối tại cổng {port}...");

                _client = await _listener.AcceptTcpClientAsync();
                _stream = _client.GetStream();

                OnSystemMessage?.Invoke("Đã có người kết nối tới!");

                // Tắt bộ lắng nghe sau khi có người kết nối thành công (ưu tiên chat 1-1)
                _listener.Stop();

                // Bắt đầu vòng lặp lắng nghe tin nhắn đến chạy ngầm
                _ = ListenForMessagesAsync();
            }
            catch (Exception ex)
            {
                OnSystemMessage?.Invoke($"Lỗi tạo phòng: {ex.Message}");
            }
        }

        /// <summary>
        /// Kết nối tới một địa chỉ IP và cổng (port) được chỉ định một cách bất đồng bộ.
        /// </summary>
        /// <param name="ipAddress">Địa chỉ IP của máy đích</param>
        /// <param name="port">Cổng mạng để kết nối</param>
        public async Task ConnectAsync(string ipAddress, int port)
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(ipAddress, port);
                _stream = _client.GetStream(); // Lấy luồng dữ liệu sau khi kết nối thành công
                
                OnSystemMessage?.Invoke("Đã kết nối thành công!");

                // Bắt đầu vòng lặp lắng nghe tin nhắn đến chạy ngầm
                _ = ListenForMessagesAsync();
            }
            catch (Exception ex)
            {
                // Thông báo lên giao diện nếu quá trình kết nối gặp sự cố
                OnSystemMessage?.Invoke($"Lỗi kết nối: {ex.Message}");
            }
        }

        /// <summary>
        /// Gửi một tin nhắn văn bản qua mạng với cơ chế Length-Prefix một cách bất đồng bộ.
        /// </summary>
        /// <param name="message">Nội dung tin nhắn cần gửi</param>
        public async Task SendMessageAsync(string message)
        {
            // Kiểm tra trạng thái kết nối và tính hợp lệ của tin nhắn trước khi gửi
            if (_client == null || !_client.Connected || string.IsNullOrWhiteSpace(message))
                return;

            try
            {
                // Chuyển đổi chuỗi văn bản thành mảng byte với chuẩn mã hóa UTF-8
                byte[] data = Encoding.UTF8.GetBytes(message);
                
                // Tạo 4 byte chứa kích thước của dữ liệu
                byte[] lengthPrefix = BitConverter.GetBytes(data.Length);
                
                // Gửi 4 byte kích thước trước
                await _stream.WriteAsync(lengthPrefix, 0, lengthPrefix.Length);
                
                // Sau đó gửi dữ liệu thực tế nội dung tin nhắn
                await _stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception)
            {
                OnSystemMessage?.Invoke("Lỗi khi gửi tin nhắn.");
            }
        }

        /// <summary>
        /// Lắng nghe và nhận liên tục các tin nhắn đóng gói theo cơ chế Length-Prefix.
        /// </summary>
        private async Task ListenForMessagesAsync()
        {
            try
            {
                while (true)
                {
                    // Bước 1: Đọc chính xác 4 byte đầu tiên để biết kích thước của tin nhắn sắp tới
                    byte[] lengthBuffer = new byte[4];
                    int lengthBytesRead = 0;
                    
                    while (lengthBytesRead < 4)
                    {
                        int read = await _stream.ReadAsync(lengthBuffer, lengthBytesRead, 4 - lengthBytesRead);
                        // Nếu đang đọc dở mà số byte bằng 0 nghĩa là đối phương ngắt kết nối
                        if (read == 0)
                        {
                            OnSystemMessage?.Invoke("Người dùng kia đã thoát.");
                            OnDisconnected?.Invoke();
                            return;
                        }
                        lengthBytesRead += read;
                    }

                    // Giải mã 4 byte thành một số nguyên (độ dài tin nhắn)
                    int dataSize = BitConverter.ToInt32(lengthBuffer, 0);

                    // Bước 2: Dựa vào độ dài vừa đọc, khởi tạo mảng để nhận đúng kích thước dữ liệu
                    byte[] dataBuffer = new byte[dataSize];
                    int totalDataRead = 0;
                    
                    while (totalDataRead < dataSize)
                    {
                        int read = await _stream.ReadAsync(dataBuffer, totalDataRead, dataSize - totalDataRead);
                        if (read == 0)
                        {
                            OnSystemMessage?.Invoke("Người dùng kia đã bị ngắt kết nối khi đang gửi dữ liệu.");
                            OnDisconnected?.Invoke();
                            return;
                        }
                        totalDataRead += read;
                    }

                    // Giải mã dữ liệu mảng byte thành chuỗi văn bản UTF-8
                    string message = Encoding.UTF8.GetString(dataBuffer, 0, dataSize);
                    
                    // Phát ra sự kiện báo hiệu có tin nhắn mới cho giao diện
                    OnMessageReceived?.Invoke(message);
                }
            }
            catch (Exception)
            {
                // Lỗi ném ra nếu bị ngắt kết nối đột ngột hoặc lỗi đường truyền
                OnDisconnected?.Invoke();
            }
        }

        /// <summary>
        /// Đóng luồng dữ liệu và ngắt kết nối client TCP để giải phóng các tài nguyên mạng.
        /// </summary>
        public void Disconnect()
        {
            _listener?.Stop();
            _stream?.Close();
            _client?.Close();
        }
    }
}