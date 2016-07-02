namespace SocketServerForApp.Server
{
    partial class Server
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
            this.btn_control = new System.Windows.Forms.Button();
            this.txt_content = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_control
            // 
            this.btn_control.Location = new System.Drawing.Point(12, 12);
            this.btn_control.Name = "btn_control";
            this.btn_control.Size = new System.Drawing.Size(461, 74);
            this.btn_control.TabIndex = 0;
            this.btn_control.Text = "啟動";
            this.btn_control.UseVisualStyleBackColor = true;
            this.btn_control.Click += new System.EventHandler(this.btn_control_Click);
            // 
            // txt_content
            // 
            this.txt_content.ForeColor = System.Drawing.Color.Green;
            this.txt_content.Location = new System.Drawing.Point(13, 93);
            this.txt_content.MaxLength = 0;
            this.txt_content.Multiline = true;
            this.txt_content.Name = "txt_content";
            this.txt_content.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_content.Size = new System.Drawing.Size(460, 215);
            this.txt_content.TabIndex = 1;
            // 
            // Server
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(485, 320);
            this.Controls.Add(this.txt_content);
            this.Controls.Add(this.btn_control);
            this.Name = "Server";
            this.Text = "Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Server_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_control;
        private System.Windows.Forms.TextBox txt_content;
    }
}