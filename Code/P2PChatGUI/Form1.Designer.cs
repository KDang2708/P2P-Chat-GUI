namespace P2PChatGUI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.RichTextBox txtChat;
        private System.Windows.Forms.TextBox txtMessage;

        private System.Windows.Forms.Button btnHost;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPort;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtIP = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtChat = new System.Windows.Forms.RichTextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();

            this.btnHost = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.lblIP = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();

            this.SuspendLayout();

            // LBL IP
            lblIP.Text = "IP:";
            lblIP.Location = new System.Drawing.Point(20, 20);
            lblIP.AutoSize = true;
            lblIP.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblIP.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);

            // IP
            txtIP.Location = new System.Drawing.Point(50, 20);
            txtIP.Size = new System.Drawing.Size(320, 30);
            txtIP.Name = "txtIP";
            txtIP.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtIP.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            txtIP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtIP.Multiline = true;
            txtIP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIP_KeyPress);

            // LBL PORT
            lblPort.Text = "Port:";
            lblPort.Location = new System.Drawing.Point(390, 20);
            lblPort.AutoSize = true;
            lblPort.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            lblPort.ForeColor = System.Drawing.Color.FromArgb(100, 100, 100);

            // PORT
            txtPort.Location = new System.Drawing.Point(440, 20);
            txtPort.Size = new System.Drawing.Size(120, 30);
            txtPort.Name = "txtPort";
            txtPort.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtPort.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            txtPort.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtPort.Multiline = true;
            txtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPort_KeyPress);

            // HOST
            btnHost.Location = new System.Drawing.Point(20, 60);
            btnHost.Size = new System.Drawing.Size(180, 35);
            btnHost.Text = "Host";
            btnHost.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnHost.BackColor = System.Drawing.Color.FromArgb(20, 20, 25);
            btnHost.ForeColor = System.Drawing.Color.White;
            btnHost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnHost.FlatAppearance.BorderSize = 0;
            btnHost.Cursor = System.Windows.Forms.Cursors.Hand;
            btnHost.Click += new System.EventHandler(this.btnHost_Click);

            // CONNECT
            btnConnect.Location = new System.Drawing.Point(210, 60);
            btnConnect.Size = new System.Drawing.Size(170, 35);
            btnConnect.Text = "Connect";
            btnConnect.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnConnect.BackColor = System.Drawing.Color.FromArgb(20, 20, 25);
            btnConnect.ForeColor = System.Drawing.Color.White;
            btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnConnect.FlatAppearance.BorderSize = 0;
            btnConnect.Cursor = System.Windows.Forms.Cursors.Hand;
            btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // DISCONNECT
            btnDisconnect.Location = new System.Drawing.Point(390, 60);
            btnDisconnect.Size = new System.Drawing.Size(170, 35);
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnDisconnect.BackColor = System.Drawing.Color.FromArgb(235, 120, 135);
            btnDisconnect.ForeColor = System.Drawing.Color.White;
            btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnDisconnect.FlatAppearance.BorderSize = 0;
            btnDisconnect.Cursor = System.Windows.Forms.Cursors.Hand;
            btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);

            // CHAT BOX
            txtChat.Location = new System.Drawing.Point(20, 110);
            txtChat.Size = new System.Drawing.Size(540, 320);
            txtChat.ReadOnly = true;
            txtChat.Name = "txtChat";
            txtChat.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtChat.BackColor = System.Drawing.Color.White;
            txtChat.BorderStyle = System.Windows.Forms.BorderStyle.None;

            // MESSAGE
            txtMessage.Location = new System.Drawing.Point(20, 445);
            txtMessage.Size = new System.Drawing.Size(430, 30);
            txtMessage.Name = "txtMessage";
            txtMessage.Font = new System.Drawing.Font("Segoe UI", 10F);
            txtMessage.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtMessage.Multiline = true;
            txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);

            // SEND
            btnSend.Location = new System.Drawing.Point(460, 445);
            btnSend.Size = new System.Drawing.Size(100, 30);
            btnSend.Text = "Send";
            btnSend.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold);
            btnSend.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            btnSend.ForeColor = System.Drawing.Color.Black;
            btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
            btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // FORM
            this.ClientSize = new System.Drawing.Size(580, 500);
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(lblIP);
            this.Controls.Add(txtIP);
            this.Controls.Add(lblPort);
            this.Controls.Add(txtPort);
            this.Controls.Add(btnHost);
            this.Controls.Add(btnConnect);
            this.Controls.Add(btnDisconnect);
            this.Controls.Add(txtChat);
            this.Controls.Add(txtMessage);
            this.Controls.Add(btnSend);

            this.Text = "P2P Chat";
            this.Load += new System.EventHandler(this.Form1_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}