namespace TeamTalkApp.NET
{
    partial class MessageDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageDlg));
            this.historyTextBox = new System.Windows.Forms.TextBox();
            this.newmsgTextBox = new System.Windows.Forms.TextBox();
            this.linkLabelSendMessage = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // historyTextBox
            // 
            this.historyTextBox.AccessibleDescription = "Otrzymane wiadomoœci";
            this.historyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.historyTextBox.Location = new System.Drawing.Point(6, 12);
            this.historyTextBox.Multiline = true;
            this.historyTextBox.Name = "historyTextBox";
            this.historyTextBox.ReadOnly = true;
            this.historyTextBox.Size = new System.Drawing.Size(484, 371);
            this.historyTextBox.TabIndex = 0;
            // 
            // newmsgTextBox
            // 
            this.newmsgTextBox.AccessibleName = "Pole wpisywania wiadomoœci";
            this.newmsgTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.newmsgTextBox.BackColor = System.Drawing.Color.White;
            this.newmsgTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.newmsgTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.newmsgTextBox.Location = new System.Drawing.Point(3, 3);
            this.newmsgTextBox.Name = "newmsgTextBox";
            this.newmsgTextBox.Size = new System.Drawing.Size(352, 15);
            this.newmsgTextBox.TabIndex = 0;
            this.newmsgTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.newmsgTextBox_KeyUp);
            // 
            // linkLabelSendMessage
            // 
            this.linkLabelSendMessage.AccessibleDescription = "";
            this.linkLabelSendMessage.AccessibleName = " ";
            this.linkLabelSendMessage.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(166)))));
            this.linkLabelSendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabelSendMessage.Cursor = System.Windows.Forms.Cursors.Default;
            this.linkLabelSendMessage.Image = global::TeamTalkApp.NET.Properties.Resources.message_admin_small;
            this.linkLabelSendMessage.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabelSendMessage.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelSendMessage.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(166)))));
            this.linkLabelSendMessage.Location = new System.Drawing.Point(365, 389);
            this.linkLabelSendMessage.Name = "linkLabelSendMessage";
            this.linkLabelSendMessage.Size = new System.Drawing.Size(125, 32);
            this.linkLabelSendMessage.TabIndex = 1;
            this.linkLabelSendMessage.TabStop = true;
            this.linkLabelSendMessage.Text = "Wyœlij wiadomoœæ";
            this.linkLabelSendMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.linkLabelSendMessage.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(87)))), ((int)(((byte)(166)))));
            this.linkLabelSendMessage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSendMessage_LinkClicked);
            this.linkLabelSendMessage.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(189)))), ((int)(((byte)(242)))));
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Location = new System.Drawing.Point(6, 392);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(1);
            this.panel1.Size = new System.Drawing.Size(354, 24);
            this.panel1.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.newmsgTextBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(352, 22);
            this.panel2.TabIndex = 0;
            // 
            // MessageDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(496, 423);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.linkLabelSendMessage);
            this.Controls.Add(this.historyTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MessageDlg";
            this.Text = "Wiadomoœæ prywatna";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox historyTextBox;
        private System.Windows.Forms.TextBox newmsgTextBox;
        private System.Windows.Forms.LinkLabel linkLabelSendMessage;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}