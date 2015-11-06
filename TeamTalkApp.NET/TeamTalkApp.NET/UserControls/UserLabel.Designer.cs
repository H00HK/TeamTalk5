namespace TeamTalkApp.NET.UserControls
{
    partial class UserLabel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserLabel));
            this.muteLinkLabel = new System.Windows.Forms.LinkLabel();
            this.allowLinkLabel = new System.Windows.Forms.LinkLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.imageListCounter = new System.Windows.Forms.ImageList(this.components);
            this.linkLabelSendMessage = new System.Windows.Forms.LinkLabel();
            this.pictureBoxMe = new System.Windows.Forms.PictureBox();
            this.moderatorPictureBox = new System.Windows.Forms.PictureBox();
            this.speakingPictureBox = new System.Windows.Forms.PictureBox();
            this.mutedPictureBox = new System.Windows.Forms.PictureBox();
            this.labelUserName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.linkLabelCounter = new System.Windows.Forms.LinkLabel();
            this.toolTipIcons = new System.Windows.Forms.ToolTip(this.components);
            this.linkLabelTransmitVideo = new System.Windows.Forms.LinkLabel();
            this.linkLabelTime = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.moderatorPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speakingPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mutedPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // muteLinkLabel
            // 
            this.muteLinkLabel.AutoSize = true;
            this.muteLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.muteLinkLabel.LinkColor = System.Drawing.Color.DodgerBlue;
            this.muteLinkLabel.Location = new System.Drawing.Point(182, 4);
            this.muteLinkLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.muteLinkLabel.Name = "muteLinkLabel";
            this.muteLinkLabel.Size = new System.Drawing.Size(41, 13);
            this.muteLinkLabel.TabIndex = 5;
            this.muteLinkLabel.TabStop = true;
            this.muteLinkLabel.Text = "Wycisz";
            this.muteLinkLabel.Visible = false;
            this.muteLinkLabel.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.muteLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.muteLinkLabel_LinkClicked);
            // 
            // allowLinkLabel
            // 
            this.allowLinkLabel.AccessibleName = " ";
            this.allowLinkLabel.AutoSize = true;
            this.allowLinkLabel.DisabledLinkColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.allowLinkLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.allowLinkLabel.LinkColor = System.Drawing.Color.DodgerBlue;
            this.allowLinkLabel.Location = new System.Drawing.Point(182, 4);
            this.allowLinkLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.allowLinkLabel.Name = "allowLinkLabel";
            this.allowLinkLabel.Size = new System.Drawing.Size(103, 13);
            this.allowLinkLabel.TabIndex = 6;
            this.allowLinkLabel.TabStop = true;
            this.allowLinkLabel.Text = "Zezwól na mówienie";
            this.allowLinkLabel.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.allowLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(0, 59);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(419, 1);
            this.panel1.TabIndex = 7;
            // 
            // imageListCounter
            // 
            this.imageListCounter.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListCounter.ImageStream")));
            this.imageListCounter.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListCounter.Images.SetKeyName(0, "notification-counter.png");
            this.imageListCounter.Images.SetKeyName(1, "notification-counter-02.png");
            this.imageListCounter.Images.SetKeyName(2, "notification-counter-03.png");
            this.imageListCounter.Images.SetKeyName(3, "notification-counter-04.png");
            this.imageListCounter.Images.SetKeyName(4, "notification-counter-05.png");
            this.imageListCounter.Images.SetKeyName(5, "notification-counter-06.png");
            this.imageListCounter.Images.SetKeyName(6, "notification-counter-07.png");
            this.imageListCounter.Images.SetKeyName(7, "notification-counter-08.png");
            this.imageListCounter.Images.SetKeyName(8, "notification-counter-09.png");
            this.imageListCounter.Images.SetKeyName(9, "notification-counter-10.png");
            this.imageListCounter.Images.SetKeyName(10, "notification-counter-11.png");
            this.imageListCounter.Images.SetKeyName(11, "notification-counter-12.png");
            this.imageListCounter.Images.SetKeyName(12, "notification-counter-13.png");
            this.imageListCounter.Images.SetKeyName(13, "notification-counter-14.png");
            this.imageListCounter.Images.SetKeyName(14, "notification-counter-15.png");
            this.imageListCounter.Images.SetKeyName(15, "notification-counter-16.png");
            this.imageListCounter.Images.SetKeyName(16, "notification-counter-17.png");
            this.imageListCounter.Images.SetKeyName(17, "notification-counter-18.png");
            this.imageListCounter.Images.SetKeyName(18, "notification-counter-19.png");
            this.imageListCounter.Images.SetKeyName(19, "notification-counter-20.png");
            // 
            // linkLabelSendMessage
            // 
            this.linkLabelSendMessage.AccessibleName = " ";
            this.linkLabelSendMessage.AutoSize = true;
            this.linkLabelSendMessage.DisabledLinkColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.linkLabelSendMessage.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelSendMessage.LinkColor = System.Drawing.Color.DodgerBlue;
            this.linkLabelSendMessage.Location = new System.Drawing.Point(182, 20);
            this.linkLabelSendMessage.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabelSendMessage.Name = "linkLabelSendMessage";
            this.linkLabelSendMessage.Size = new System.Drawing.Size(95, 13);
            this.linkLabelSendMessage.TabIndex = 10;
            this.linkLabelSendMessage.TabStop = true;
            this.linkLabelSendMessage.Text = "Napisz wiadomość";
            this.linkLabelSendMessage.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.linkLabelSendMessage.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelSendMessage_LinkClicked);
            // 
            // pictureBoxMe
            // 
            this.pictureBoxMe.Image = global::TeamTalkApp.NET.Properties.Resources.myself;
            this.pictureBoxMe.Location = new System.Drawing.Point(22, 2);
            this.pictureBoxMe.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxMe.Name = "pictureBoxMe";
            this.pictureBoxMe.Size = new System.Drawing.Size(42, 42);
            this.pictureBoxMe.TabIndex = 9;
            this.pictureBoxMe.TabStop = false;
            this.toolTipIcons.SetToolTip(this.pictureBoxMe, "Ty");
            this.pictureBoxMe.Visible = false;
            // 
            // moderatorPictureBox
            // 
            this.moderatorPictureBox.Image = global::TeamTalkApp.NET.Properties.Resources.moderator;
            this.moderatorPictureBox.Location = new System.Drawing.Point(68, 2);
            this.moderatorPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.moderatorPictureBox.Name = "moderatorPictureBox";
            this.moderatorPictureBox.Size = new System.Drawing.Size(42, 42);
            this.moderatorPictureBox.TabIndex = 4;
            this.moderatorPictureBox.TabStop = false;
            this.toolTipIcons.SetToolTip(this.moderatorPictureBox, "Moderator");
            this.moderatorPictureBox.Visible = false;
            // 
            // speakingPictureBox
            // 
            this.speakingPictureBox.Image = global::TeamTalkApp.NET.Properties.Resources.speaking;
            this.speakingPictureBox.Location = new System.Drawing.Point(87, 2);
            this.speakingPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.speakingPictureBox.Name = "speakingPictureBox";
            this.speakingPictureBox.Size = new System.Drawing.Size(42, 42);
            this.speakingPictureBox.TabIndex = 3;
            this.speakingPictureBox.TabStop = false;
            this.toolTipIcons.SetToolTip(this.speakingPictureBox, "Mówi");
            this.speakingPictureBox.Visible = false;
            // 
            // mutedPictureBox
            // 
            this.mutedPictureBox.Image = global::TeamTalkApp.NET.Properties.Resources.blocked;
            this.mutedPictureBox.Location = new System.Drawing.Point(133, 2);
            this.mutedPictureBox.Margin = new System.Windows.Forms.Padding(2);
            this.mutedPictureBox.Name = "mutedPictureBox";
            this.mutedPictureBox.Size = new System.Drawing.Size(42, 42);
            this.mutedPictureBox.TabIndex = 1;
            this.mutedPictureBox.TabStop = false;
            this.toolTipIcons.SetToolTip(this.mutedPictureBox, "Wyciszony");
            this.mutedPictureBox.Visible = false;
            // 
            // labelUserName
            // 
            this.labelUserName.AccessibleName = " ";
            this.labelUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelUserName.Image = global::TeamTalkApp.NET.Properties.Resources.username;
            this.labelUserName.Location = new System.Drawing.Point(-1, 0);
            this.labelUserName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelUserName.Name = "labelUserName";
            this.labelUserName.Size = new System.Drawing.Size(176, 20);
            this.labelUserName.TabIndex = 0;
            this.labelUserName.Text = "label1";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TeamTalkApp.NET.Properties.Resources.queue;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(42, 42);
            this.pictureBox1.TabIndex = 11;
            this.pictureBox1.TabStop = false;
            this.toolTipIcons.SetToolTip(this.pictureBox1, "Zgłoszona chęć wypowiedzi");
            this.pictureBox1.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.moderatorPictureBox);
            this.flowLayoutPanel1.Controls.Add(this.pictureBoxMe);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(307, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.flowLayoutPanel1.Size = new System.Drawing.Size(112, 46);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.panel2);
            this.flowLayoutPanel2.Controls.Add(this.speakingPictureBox);
            this.flowLayoutPanel2.Controls.Add(this.mutedPictureBox);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 13);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(177, 47);
            this.flowLayoutPanel2.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.linkLabelCounter);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(79, 46);
            this.panel2.TabIndex = 14;
            // 
            // linkLabelCounter
            // 
            this.linkLabelCounter.AccessibleDescription = "0";
            this.linkLabelCounter.AccessibleName = "Miejsce w kolecje";
            this.linkLabelCounter.AutoSize = true;
            this.linkLabelCounter.DisabledLinkColor = System.Drawing.Color.Red;
            this.linkLabelCounter.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.linkLabelCounter.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelCounter.LinkColor = System.Drawing.Color.Red;
            this.linkLabelCounter.Location = new System.Drawing.Point(44, 7);
            this.linkLabelCounter.Name = "linkLabelCounter";
            this.linkLabelCounter.Size = new System.Drawing.Size(26, 29);
            this.linkLabelCounter.TabIndex = 15;
            this.linkLabelCounter.TabStop = true;
            this.linkLabelCounter.Text = "0";
            this.linkLabelCounter.VisitedLinkColor = System.Drawing.Color.Red;
            // 
            // linkLabelTransmitVideo
            // 
            this.linkLabelTransmitVideo.AccessibleName = " ";
            this.linkLabelTransmitVideo.AutoSize = true;
            this.linkLabelTransmitVideo.DisabledLinkColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.linkLabelTransmitVideo.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelTransmitVideo.LinkColor = System.Drawing.Color.DodgerBlue;
            this.linkLabelTransmitVideo.Location = new System.Drawing.Point(182, 43);
            this.linkLabelTransmitVideo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabelTransmitVideo.Name = "linkLabelTransmitVideo";
            this.linkLabelTransmitVideo.Size = new System.Drawing.Size(70, 13);
            this.linkLabelTransmitVideo.TabIndex = 14;
            this.linkLabelTransmitVideo.TabStop = true;
            this.linkLabelTransmitVideo.Text = "Prześlij wideo";
            this.linkLabelTransmitVideo.Visible = false;
            this.linkLabelTransmitVideo.VisitedLinkColor = System.Drawing.Color.DodgerBlue;
            this.linkLabelTransmitVideo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked_1);
            // 
            // linkLabelTime
            // 
            this.linkLabelTime.AutoSize = true;
            this.linkLabelTime.DisabledLinkColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.linkLabelTime.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.linkLabelTime.LinkColor = System.Drawing.Color.Firebrick;
            this.linkLabelTime.Location = new System.Drawing.Point(383, 4);
            this.linkLabelTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.linkLabelTime.Name = "linkLabelTime";
            this.linkLabelTime.Size = new System.Drawing.Size(34, 13);
            this.linkLabelTime.TabIndex = 15;
            this.linkLabelTime.TabStop = true;
            this.linkLabelTime.Text = "00:00";
            this.linkLabelTime.Visible = false;
            this.linkLabelTime.VisitedLinkColor = System.Drawing.Color.Firebrick;
            // 
            // UserLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.Controls.Add(this.linkLabelTime);
            this.Controls.Add(this.linkLabelTransmitVideo);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.linkLabelSendMessage);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.allowLinkLabel);
            this.Controls.Add(this.muteLinkLabel);
            this.Controls.Add(this.labelUserName);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserLabel";
            this.Size = new System.Drawing.Size(419, 58);
            this.MouseEnter += new System.EventHandler(this.UserLabel_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.UserLabel_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.moderatorPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speakingPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mutedPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelUserName;
        private System.Windows.Forms.PictureBox mutedPictureBox;
        private System.Windows.Forms.PictureBox speakingPictureBox;
        private System.Windows.Forms.PictureBox moderatorPictureBox;
        private System.Windows.Forms.LinkLabel muteLinkLabel;
        private System.Windows.Forms.LinkLabel allowLinkLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ImageList imageListCounter;
        private System.Windows.Forms.PictureBox pictureBoxMe;
        private System.Windows.Forms.LinkLabel linkLabelSendMessage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolTip toolTipIcons;
        private System.Windows.Forms.LinkLabel linkLabelTransmitVideo;
        private System.Windows.Forms.LinkLabel linkLabelCounter;
        private System.Windows.Forms.LinkLabel linkLabelTime;
    }
}
