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

            this.SuspendLayout();

            // IP
            txtIP.Location = new System.Drawing.Point(20, 15);
            txtIP.Size = new System.Drawing.Size(120, 23);
            txtIP.Name = "txtIP";

            // PORT
            txtPort.Location = new System.Drawing.Point(150, 15);
            txtPort.Size = new System.Drawing.Size(70, 23);
            txtPort.Name = "txtPort";

            // HOST
            btnHost.Location = new System.Drawing.Point(230, 13);
            btnHost.Size = new System.Drawing.Size(70, 25);
            btnHost.Text = "Host";
            btnHost.Click += new System.EventHandler(this.btnHost_Click);

            // CONNECT
            btnConnect.Location = new System.Drawing.Point(310, 13);
            btnConnect.Size = new System.Drawing.Size(80, 25);
            btnConnect.Text = "Connect";
            btnConnect.Click += new System.EventHandler(this.btnConnect_Click);

            // DISCONNECT
            btnDisconnect.Location = new System.Drawing.Point(400, 13);
            btnDisconnect.Size = new System.Drawing.Size(90, 25);
            btnDisconnect.Text = "Disconnect";
            btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);

            // CHAT BOX
            txtChat.Location = new System.Drawing.Point(20, 50);
            txtChat.Size = new System.Drawing.Size(470, 250);
            txtChat.ReadOnly = true;
            txtChat.Name = "txtChat";

            // MESSAGE
            txtMessage.Location = new System.Drawing.Point(20, 310);
            txtMessage.Size = new System.Drawing.Size(370, 23);
            txtMessage.Name = "txtMessage";
            txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);

            // SEND
            btnSend.Location = new System.Drawing.Point(400, 308);
            btnSend.Size = new System.Drawing.Size(90, 25);
            btnSend.Text = "Send";
            btnSend.Click += new System.EventHandler(this.btnSend_Click);

            // FORM
            this.ClientSize = new System.Drawing.Size(520, 360);
            this.Controls.Add(txtIP);
            this.Controls.Add(txtPort);
            this.Controls.Add(btnHost);
            this.Controls.Add(btnConnect);
            this.Controls.Add(btnDisconnect);
            this.Controls.Add(txtChat);
            this.Controls.Add(txtMessage);
            this.Controls.Add(btnSend);

            this.Text = "P2P Chat - Lê Anh Kiệt";
            this.Load += new System.EventHandler(this.Form1_Load);

            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}