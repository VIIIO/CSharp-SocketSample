namespace SocketServerForApp.Client
{
    partial class Client
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_connect = new System.Windows.Forms.Button();
            this.txt_account = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_content = new System.Windows.Forms.TextBox();
            this.cbo_account = new System.Windows.Forms.ListBox();
            this.lbl_online = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_message = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.btn_disconnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_connect
            // 
            this.btn_connect.Location = new System.Drawing.Point(319, 8);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(120, 31);
            this.btn_connect.TabIndex = 0;
            this.btn_connect.Text = "進入聊天室";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // txt_account
            // 
            this.txt_account.Location = new System.Drawing.Point(45, 14);
            this.txt_account.Name = "txt_account";
            this.txt_account.Size = new System.Drawing.Size(268, 21);
            this.txt_account.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "暱稱";
            // 
            // txt_content
            // 
            this.txt_content.ForeColor = System.Drawing.Color.Black;
            this.txt_content.Location = new System.Drawing.Point(12, 67);
            this.txt_content.MaxLength = 0;
            this.txt_content.Multiline = true;
            this.txt_content.Name = "txt_content";
            this.txt_content.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_content.Size = new System.Drawing.Size(674, 219);
            this.txt_content.TabIndex = 3;
            // 
            // cbo_account
            // 
            this.cbo_account.FormattingEnabled = true;
            this.cbo_account.ItemHeight = 12;
            this.cbo_account.Location = new System.Drawing.Point(698, 66);
            this.cbo_account.Name = "cbo_account";
            this.cbo_account.Size = new System.Drawing.Size(120, 220);
            this.cbo_account.TabIndex = 4;
            // 
            // lbl_online
            // 
            this.lbl_online.AutoSize = true;
            this.lbl_online.Location = new System.Drawing.Point(696, 51);
            this.lbl_online.Name = "lbl_online";
            this.lbl_online.Size = new System.Drawing.Size(53, 12);
            this.lbl_online.TabIndex = 5;
            this.lbl_online.Text = "在線列表";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "聊天內容";
            // 
            // txt_message
            // 
            this.txt_message.ForeColor = System.Drawing.Color.Black;
            this.txt_message.Location = new System.Drawing.Point(12, 311);
            this.txt_message.MaxLength = 0;
            this.txt_message.Multiline = true;
            this.txt_message.Name = "txt_message";
            this.txt_message.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_message.Size = new System.Drawing.Size(674, 91);
            this.txt_message.TabIndex = 7;
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(698, 311);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(120, 91);
            this.btn_send.TabIndex = 8;
            this.btn_send.Text = "發送";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // btn_disconnect
            // 
            this.btn_disconnect.Location = new System.Drawing.Point(446, 8);
            this.btn_disconnect.Name = "btn_disconnect";
            this.btn_disconnect.Size = new System.Drawing.Size(114, 31);
            this.btn_disconnect.TabIndex = 9;
            this.btn_disconnect.Text = "斷開";
            this.btn_disconnect.UseVisualStyleBackColor = true;
            this.btn_disconnect.Click += new System.EventHandler(this.btn_disconnect_Click);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(828, 424);
            this.Controls.Add(this.btn_disconnect);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.txt_message);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_online);
            this.Controls.Add(this.cbo_account);
            this.Controls.Add(this.txt_content);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_account);
            this.Controls.Add(this.btn_connect);
            this.Name = "Client";
            this.Text = "Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Client_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.TextBox txt_account;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_content;
        private System.Windows.Forms.ListBox cbo_account;
        private System.Windows.Forms.Label lbl_online;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_message;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.Button btn_disconnect;
    }
}