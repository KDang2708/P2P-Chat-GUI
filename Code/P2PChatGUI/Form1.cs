using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using P2PChatGUI.Core;

namespace P2PChatGUI
{
    public partial class Form1 : Form
    {
        private ChatNetworkCore network;

        public Form1()
        {
            InitializeComponent();
            network = new ChatNetworkCore();

            btnHost.EnabledChanged += Button_EnabledChanged;
            btnConnect.EnabledChanged += Button_EnabledChanged;
            btnDisconnect.EnabledChanged += Button_EnabledChanged;
            btnSend.EnabledChanged += Button_EnabledChanged;

            btnSend.Enabled = false;
            btnDisconnect.Enabled = false;

            network.OnMessageReceived += (msg) =>
            {
                Invoke(new Action(() => AppendChatText("Đối phương", msg, Color.FromArgb(100, 100, 100), HorizontalAlignment.Left)));
            };

            network.OnSystemMessage += (msg) =>
            {
                Invoke(new Action(() =>
                {
                    if (msg.StartsWith("Lỗi") || msg.Contains("ngắt") || msg.Contains("thoát"))
                    {
                        AppendStyledText(msg, Color.FromArgb(235, 87, 87), Color.FromArgb(253, 237, 237), Color.FromArgb(180, 50, 50));
                    }
                    else if (msg.Contains("thành công") || msg.Contains("tới") || msg.Contains("Host"))
                    {
                        AppendStyledText(msg, Color.FromArgb(39, 174, 96), Color.FromArgb(232, 248, 245), Color.FromArgb(39, 174, 96));
                    }
                    else
                    {
                        AppendStyledText(msg, Color.FromArgb(45, 156, 219), Color.FromArgb(234, 242, 248), Color.FromArgb(41, 128, 185));
                    }
                }));
            };

            network.OnDisconnected += () =>
            {
                Invoke(new Action(() =>
                {
                    AppendStyledText("Mất kết nối", Color.FromArgb(235, 87, 87), Color.FromArgb(253, 237, 237), Color.FromArgb(180, 50, 50));
                    btnSend.Enabled = false;
                    btnHost.Enabled = true;
                    btnConnect.Enabled = true;
                    btnDisconnect.Enabled = false;
                }));
            };
        }

        private void Button_EnabledChanged(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Enabled)
                {
                    if (btn == btnDisconnect)
                    {
                        btn.BackColor = Color.FromArgb(235, 120, 135);
                        btn.ForeColor = Color.White;
                    }
                    else if (btn == btnSend)
                    {
                        btn.BackColor = Color.FromArgb(245, 245, 245);
                        btn.ForeColor = Color.Black;
                    }
                    else // Host và Connect
                    {
                        btn.BackColor = Color.FromArgb(20, 20, 25);
                        btn.ForeColor = Color.White;
                    }
                }
                else
                {
                    btn.BackColor = Color.FromArgb(230, 230, 230);
                    btn.ForeColor = Color.Gray;
                }
            }
        }

        private void AppendStyledText(string text, Color barColor, Color bgColor, Color textColor)
        {
            txtChat.SelectionStart = txtChat.TextLength;
            txtChat.SelectionLength = 0;
            txtChat.SelectionAlignment = HorizontalAlignment.Left;
            txtChat.SelectionBackColor = bgColor;

            txtChat.SelectionFont = new Font("Segoe UI", 12F, FontStyle.Bold);
            txtChat.SelectionColor = barColor;
            txtChat.AppendText(" ▌");

            txtChat.SelectionFont = new Font("Segoe UI", 10F, FontStyle.Regular);
            txtChat.SelectionColor = textColor;
            txtChat.AppendText(" " + text.PadRight(100) + "\n\n");

            txtChat.SelectionBackColor = txtChat.BackColor;
            txtChat.SelectionStart = txtChat.TextLength;
            txtChat.ScrollToCaret();
        }

        private void AppendChatText(string prefix, string msg, Color prefixColor, HorizontalAlignment alignment)
        {
            txtChat.SelectionStart = txtChat.TextLength;
            txtChat.SelectionLength = 0;
            txtChat.SelectionAlignment = alignment;
            txtChat.SelectionColor = prefixColor;
            txtChat.AppendText(prefix + ": ");
            txtChat.SelectionColor = Color.FromArgb(30, 30, 30);
            txtChat.AppendText(msg + "\n\n");
            txtChat.SelectionStart = txtChat.TextLength;
            txtChat.ScrollToCaret();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtIP.Text = "127.0.0.1";
            txtPort.Text = "8888";
        }

        private async void btnHost_Click(object sender, EventArgs e)
        {
            btnHost.Enabled = false;
            btnConnect.Enabled = false;
            btnDisconnect.Enabled = true;

            if (await network.StartListeningAsync(txtIP.Text, int.Parse(txtPort.Text)))
            {
                AppendStyledText("Bạn đang là Host", Color.FromArgb(39, 174, 96), Color.FromArgb(240, 255, 240), Color.FromArgb(39, 174, 96));
                btnSend.Enabled = true;
                txtMessage.Focus();
            }
            else
            {
                btnHost.Enabled = true;
                btnConnect.Enabled = true;
                btnDisconnect.Enabled = false;
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            btnHost.Enabled = false;
            btnDisconnect.Enabled = true;

            if (await network.ConnectAsync(txtIP.Text, int.Parse(txtPort.Text)))
            {
                AppendStyledText("Đã kết nối", Color.FromArgb(39, 174, 96), Color.FromArgb(240, 255, 240), Color.FromArgb(39, 174, 96));
                btnSend.Enabled = true;
                txtMessage.Focus();
            }
            else
            {
                btnConnect.Enabled = true;
                btnHost.Enabled = true;
                btnDisconnect.Enabled = false;
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            await SendMessage();
        }

        private async Task SendMessage()
        {
            string msg = txtMessage.Text.Trim();
            if (msg == "") return;

            await network.SendMessageAsync(msg);

            AppendChatText("Bạn", msg, Color.FromArgb(100, 100, 100), HorizontalAlignment.Right);

            txtMessage.Clear();
        }

        private async void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Luôn chặn tiếng "ting" khi ấn Enter

                if (btnSend.Enabled) // Chỉ gửi nếu nút send đang được bật (đã kết nối)
                {
                    await SendMessage();
                }
            }
        }

        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            await network.DisconnectAsync();

            AppendStyledText("Bạn đã ngắt kết nối", Color.FromArgb(235, 87, 87), Color.FromArgb(253, 237, 237), Color.FromArgb(180, 50, 50));

            btnSend.Enabled = false;
            btnHost.Enabled = true;
            btnConnect.Enabled = true;
            btnDisconnect.Enabled = false;
        }

        private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép nhập số và các phím điều khiển (như Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            // IP thường chỉ có số và dấu phẩy/chấm
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }
    }
}