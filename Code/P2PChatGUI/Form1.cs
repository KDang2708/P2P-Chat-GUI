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

            btnSend.Enabled = false;

            network.OnMessageReceived += (msg) =>
            {
                Invoke(new Action(() =>
                {
                    txtChat.SelectionColor = Color.Black;
                    txtChat.AppendText($"[{DateTime.Now:HH:mm}] Đối phương: {msg}\n");
                    txtChat.ScrollToCaret();
                }));
            };

            network.OnSystemMessage += (msg) =>
            {
                Invoke(new Action(() =>
                {
                    txtChat.SelectionColor = Color.Gray;
                    txtChat.AppendText($"[System] {msg}\n");
                }));
            };

            network.OnDisconnected += () =>
            {
                Invoke(new Action(() =>
                {
                    txtChat.SelectionColor = Color.Red;
                    txtChat.AppendText("⚠ Mất kết nối\n");
                    btnSend.Enabled = false;
                    btnHost.Enabled = true;
                    btnConnect.Enabled = true;
                }));
            };
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

            if (await network.StartListeningAsync(txtIP.Text, int.Parse(txtPort.Text)))
            {
                txtChat.AppendText("✔ Bạn đang là Host\n");
                btnSend.Enabled = true;
                txtMessage.Focus();
            }
            else
            {
                btnHost.Enabled = true;
                btnConnect.Enabled = true;
            }
        }

        private async void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
            btnHost.Enabled = false;

            if (await network.ConnectAsync(txtIP.Text, int.Parse(txtPort.Text)))
            {
                txtChat.AppendText("✔ Đã kết nối\n");
                btnSend.Enabled = true;
                txtMessage.Focus();
            }
            else
            {
                btnConnect.Enabled = true;
                btnHost.Enabled = true;
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

            txtChat.SelectionColor = Color.Blue;
            txtChat.AppendText($"[{DateTime.Now:HH:mm}] Bạn: {msg}\n");

            txtMessage.Clear();
        }

        private async void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                await SendMessage();
                e.SuppressKeyPress = true;
            }
        }

        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            await network.DisconnectAsync();

            txtChat.SelectionColor = Color.Red;
            txtChat.AppendText("❌ Bạn đã ngắt kết nối\n");

            btnSend.Enabled = false;
            btnHost.Enabled = true;
            btnConnect.Enabled = true;
        }
    }
}