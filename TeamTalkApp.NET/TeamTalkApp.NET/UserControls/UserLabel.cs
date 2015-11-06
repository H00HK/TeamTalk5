using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TeamTalkApp.NET.UserControls
{
    public partial class UserLabel : UserControl
    {
        public UserLabel()
        {
            InitializeComponent();
        }

        public Boolean Moderator
        {
            get { return moderatorPictureBox.Visible;  }
            set
            {
                moderatorPictureBox.Visible = value;
                updateDisplay();
            }
        }

        public Boolean Speaking
        {
            get { return speakingPictureBox.Visible; }
            set { speakingPictureBox.Visible = value; }
        }

        public Boolean Muted
        {
            get { return mutedPictureBox.Visible;}
            set
            {
                mutedPictureBox.Visible = value;
                updateLinkLabel();
            }
        }

        private void updateLinkLabel()
        {
            if (Muted && Classroom)
            {
                if (!Me) allowLinkLabel.Links[0].Enabled = true;
                allowLinkLabel.Text = "Zazwól na mówienie";
            }
            else
            {

                if (!Me) allowLinkLabel.Links[0].Enabled = true;
                
                if (Classroom)
                    allowLinkLabel.Text = "Wycisz";
                else
                {
                    if (Requested)
                        allowLinkLabel.Text = "Zezwól na mówienie";
                    else
                    {
                        allowLinkLabel.Links[0].Enabled = false;
                    }
                }
            }
        }

        public Boolean Requested
        {
            get { return linkLabelCounter.Visible; }
            set
            {
                linkLabelCounter.Visible = value;
                updateLinkLabel();
            }
        }

        public Boolean Me
        {
            get { return pictureBoxMe.Visible; }
            set
            {
                pictureBoxMe.Visible = value;
                updateDisplay();
            }
        }

        public void setIsCurrentUserModerator(Boolean isModerator)
        {
            ModeratorView = isModerator;
            updateDisplay();
        }

        private void updateDisplay()
        {
            updateLinkLabel();
            if (!ModeratorView || Me)
            {
                muteLinkLabel.Visible = false;
                muteLinkLabel.Enabled = false;
                allowLinkLabel.Visible = false;
                allowLinkLabel.Links[0].Enabled = false;
                
                linkLabelSendMessage.Visible = false;
                linkLabelSendMessage.Links[0].Enabled = false;
                linkLabelTransmitVideo.Visible = false;
                linkLabelTransmitVideo.Links[0].Enabled = false;
            }
            if (ModeratorView && !Me)
            {
                allowLinkLabel.Visible = true;
                allowLinkLabel.Links[0].Enabled = true;

                //linkLabelSendMessage.Visible = false;
                linkLabelSendMessage.Visible = true;
                linkLabelSendMessage.Links[0].Enabled = true;
            
            }
            if (ModeratorView && Me)
            {
                linkLabelTransmitVideo.Visible = true;
                linkLabelTransmitVideo.Links[0].Enabled = true;
            }
            if (ModeratorView)
            {
                linkLabelTime.Visible = true;
            }
        }

        public void SetSpeakingTime(TimeSpan time)
        {
            linkLabelTime.Text = time.ToString("mm\\:ss");
        }

        public String UserName
        {
            get { return labelUserName.Text; }
            set { 
                labelUserName.Text = value;
                muteLinkLabel.AccessibleName = value + " wycisz";
                muteLinkLabel.AccessibleDescription = value + " wycisz";
                allowLinkLabel.AccessibleName = value + " zezwól na mówienie";
                allowLinkLabel.AccessibleDescription = value + " zezwól na mówienie"; 
            }
        }

        public event EventHandler MuteSelectedUser;
        public event EventHandler AllowSelectedUser;
        public event EventHandler BlockSelectedUser;
        public event EventHandler MessageSelectedUser;
        public event EventHandler TransmitVideo;

        private void muteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EventHandler handler = MuteSelectedUser;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EventHandler handler = AllowSelectedUser;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public void SetNumberIcon(int number)
        {
            if (number == -1) { linkLabelCounter.Text = (number + 1).ToString(); linkLabelCounter.TabStop = false; pictureBox1.Visible = false; panel2.Visible = false; Requested = false; }
            else { linkLabelCounter.Text = (number + 1).ToString(); linkLabelCounter.TabStop = true; linkLabelCounter.AccessibleDescription = (number + 1).ToString(); pictureBox1.Visible = true; panel2.Visible = true; Requested = true; }
        }

        private void UserLabel_MouseEnter(object sender, EventArgs e)
        {
        }

        private void UserLabel_MouseLeave(object sender, EventArgs e)
        {
        }

        private void linkLabelSendMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EventHandler handler = MessageSelectedUser;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            EventHandler handler = TransmitVideo;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public bool Classroom { get; set; }

        public bool ModeratorView { get; set; }
    }
}
