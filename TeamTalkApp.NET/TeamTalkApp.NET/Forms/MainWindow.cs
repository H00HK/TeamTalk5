/*
 * Copyright (c) 2005-2014, BearWare.dk
 * 
 * Contact Information:
 *
 * Bjoern D. Rasmussen
 * Skanderborgvej 40 4-2
 * DK-8000 Aarhus C
 * Denmark
 * Email: contact@bearware.dk
 * Phone: +45 20 20 54 59
 * Web: http://www.bearware.dk
 *
 * This source code is part of the TeamTalk 5 SDK owned by
 * BearWare.dk. All copyright statements may not be removed 
 * or altered from any source distribution. If you use this
 * software in a product, an acknowledgment in the product 
 * documentation is required.
 *
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using BearWare;
using TeamTalkApp.Utils;
using TeamTalkLib.Settings;
using NLog;

namespace TeamTalkApp.NET
{

    public partial class MainWindow : Form
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        TeamTalk ttclient;
        public static ChannelsView channels;
        UsersView users;
        FilesView files;
        ConnectionSettings commonSettings;

        Dictionary<int, MessageDlg> msgdialogs;
        Dictionary<int, VideoDlg> viddialogs;
        Dictionary<int, DesktopDlg> desktopdialogs;

        ClientStatistics statistics;

        Settings settings;

        IntPtr hShareWnd = IntPtr.Zero;
        BitmapFormat bmpShareFormat = BitmapFormat.BMP_NONE;

        //We want to pass the same bitmap reference every time so we don't
        //waste memory reallocating a new bitmap every time we call 
        //ttclient.GetUserVideoFrame
        Bitmap local_bitmap = null;

        public MainWindow(ConnectionSettings commonSettings)
        {

            try {

                InitializeComponent();

                /* Set license information before creating the client instance */
                //TeamTalk.SetLicenseInformation("", "");

                msgdialogs = new Dictionary<int, MessageDlg>();
                viddialogs = new Dictionary<int, VideoDlg>();
                desktopdialogs = new Dictionary<int, DesktopDlg>();



                /* we pass 'false' to poll_events since we don't want to 
                 * manually process events using ttclient.GetMessage */
                this.ttclient = new TeamTalk(false);
                this.users = new UsersView(ttclient, userListFlowLayoutPanel, commonSettings);
                MainWindow.channels = new ChannelsView(ttclient, channelTreeView);
                this.settings = new Settings();
                transmitsVideo = false;
                this.commonSettings = commonSettings;

                this.users.MessageUser += users_MessageUser;
                this.users.ToggleVideoTransmission += users_ToggleVideoTransmission;
                this.users.InExpertChannel += users_InExpertChannel;
                this.users.InOriginChannel += users_InOriginChannel;
                //get default devices
                TeamTalk.GetDefaultSoundDevices(ref settings.sndinputid, ref settings.sndoutputid);

                ttclient.OnConnectionSuccess += new TeamTalk.Connection(Log_OnConnectionSuccess);
                ttclient.OnConnectionFailed += new TeamTalk.Connection(ttclient_OnConnectionFailed);
                ttclient.OnConnectionLost += new TeamTalk.Connection(ttclient_OnConnectionLost);

                ttclient.OnCmdProcessing += new TeamTalk.CommandProcessing(ttclient_OnCmdProcessing);
                ttclient.OnCmdError += new TeamTalk.CommandError(ttclient_OnCmdError);
                ttclient.OnCmdMyselfLoggedIn += new TeamTalk.MyselfLoggedIn(Join_Channel_OnCmdMyselfLoggedIn);
                ttclient.OnCmdMyselfLoggedOut += new TeamTalk.MyselfLoggedOut(ttclient_OnCmdMyselfLoggedOut);
                ttclient.OnCmdUserLoggedIn += new TeamTalk.UserUpdate(ttclient_OnCmdUserLoggedIn);
                ttclient.OnCmdUserJoinedChannel += new TeamTalk.UserUpdate(ttclient_OnCmdUserJoinedChannel);
                ttclient.OnCmdUserLeftChannel += new TeamTalk.UserUpdate(ttclient_OnCmdUserLeftChannel);
                ttclient.OnCmdUserTextMessage += new TeamTalk.UserTextMessage(ttclient_OnCmdUserTextMessage);
                ttclient.OnCmdChannelNew += new TeamTalk.ChannelUpdate(ttclient_OnCmdChannelNew);
                ttclient.OnCmdChannelUpdate += new TeamTalk.ChannelUpdate(ttclient_OnCmdChannelUpdate);
                ttclient.OnCmdChannelRemove += new TeamTalk.ChannelUpdate(ttclient_OnCmdChannelRemove);

                ttclient.OnInternalError += new TeamTalk.ErrorOccured(ttclient_OnInternalError);
                ttclient.OnHotKeyToggle += new TeamTalk.HotKeyToggle(ttclient_OnHotKeyToggle);
                ttclient.OnUserVideoCapture += new TeamTalk.UserVideoFrame(ttclient_OnUserVideoCapture);
                ttclient.OnStreamMediaFile += new TeamTalk.StreamMediaFile(ttclient_OnStreamMediaFile);
                ttclient.OnUserRecordMediaFile += new TeamTalk.UserRecordMediaFile(ttclient_OnUserRecordMediaFile);
                ttclient.OnUserAudioBlock += new TeamTalk.NewAudioBlock(ttclient_OnUserAudioBlock);
                ttclient.OnUserDesktopInput += new TeamTalk.UserDesktopInput(ttclient_OnUserDesktopInput);
                ttclient.OnFileTransfer += new TeamTalk.FileTransferUpdate(ttclient_OnFileTransfer);
                ttclient.OnUserDesktopWindow += new TeamTalk.NewDesktopWindow(ttclient_OnUserDesktopWindow);

                ttclient.OnVoiceActivation += ttclient_OnVoiceActivation;

                ttclient.SetSoundOutputVolume(SoundLevel.SOUND_VOLUME_MAX);
                ttclient.SetSoundInputGainLevel(SoundLevel.SOUND_GAIN_MAX);

                //ttclient.EnableVoiceTransmission(true);

                //Forms.AutoMicroConf auto = new Forms.AutoMicroConf(ttclient, commonSettings);
                //auto.ShowDialog();

                if (false)
                {

                    SpeexDSP spxdsp = new SpeexDSP(true);
                    
                    spxdsp.bEnableAGC = Properties.Settings.Default.bEnableAGC;
                    spxdsp.bEnableDenoise = false; //Properties.Settings.Default.bEnableDenoise;
                    spxdsp.bEnableEchoCancellation = false;// Properties.Settings.Default.bEnableEchoCancellation;
                    //ttclient.SetSoundInputPreprocess(spxdsp);
                    //ttclient.SetSoundOutputVolume(24000);
                    ttclient.SetSoundInputGainLevel(Properties.Settings.Default.AudioInLevel);
                    if (!InitInputOutputSound(ttclient))
                    {
                        MessageBox.Show("Problem z inicjalizacj¹ ustawieñ urz¹dzeñ wejœcia i wyjœcia");
                    }
                }
                else
                {
                    Forms.AutoMicroConf auto = new Forms.AutoMicroConf(ttclient, commonSettings, settings);
                    auto.ShowDialog();

                    SpeexDSP spxdsp = new SpeexDSP(true);
                    spxdsp.bEnableAGC = Properties.Settings.Default.bEnableAGC;
                    spxdsp.bEnableDenoise = Properties.Settings.Default.bEnableDenoise;
                    spxdsp.bEnableEchoCancellation = Properties.Settings.Default.bEnableEchoCancellation;
                    ttclient.SetSoundInputPreprocess(spxdsp);
                    
                    //ttclient.SetSoundOutputVolume(24000);
                    ttclient.SetSoundInputGainLevel(Properties.Settings.Default.AudioInLevel);
                    if (!InitInputOutputSound(ttclient))
                    {
                        MessageBox.Show("Problem z inicjalizacj¹ ustawieñ urz¹dzeñ wejœcia i wyjœcia");
                    }

                }

                if (!Properties.Settings.Default.HotKeySet)
                {
                    Properties.Settings.Default.HotKeys.Clear();
                    HotKeyDlg dlg = new HotKeyDlg(ttclient);
                    dlg.ShowDialog();
                    Properties.Settings.Default.HotKeySet = true;
                    Properties.Settings.Default.Save();
                }

                List<int> keys = new List<int>();
                StringBuilder keyString = new StringBuilder();
                foreach (String key in Properties.Settings.Default.HotKeys)
                {
                    keys.Add(Convert.ToInt32(key));
                    if (key == "162")
                        keyString.Append("Ctrl");
                    else if (key == "1")
                        keyString.Append("LPM");
                    else if (key == "2")
                        keyString.Append("PPM");
                    else if (key == "4")
                        keyString.Append("ŒPM");
                    else if (key == "160")
                        keyString.Append("Shift");
                    else if (key == "164")
                        keyString.Append("Alt");
                    else                    
                        keyString.Append((char)Convert.ToInt32(key));
                    keyString.Append("+");
                }
                keyString.Remove(keyString.Length - 1, 1);
                ttclient.HotKey_Register((int)HotKey.HOTKEY_PUSHTOTALK, keys.ToArray());
                
                
                linkLabelSpeak.Text = String.Format("Aby mówiæ, wciœnij {0}", keyString.ToString());

            } catch(Exception exc)
            {
                log.Error(exc.Message);
            }

        }

        void ttclient_OnVoiceActivation(bool bVoiceActive)
        {
            speakingPictureBox.Visible = bVoiceActive;
            pictureBoxSilent.Visible = !(bVoiceActive);

            users.VoiceEnabled = bVoiceActive;
        }

        void users_InOriginChannel(object sender, EventArgs e)
        {
            linkLabelLocation.Text = "Znajdujesz siê w pokoju dyskusyjnym";
        }

        void users_InExpertChannel(object sender, EventArgs e)
        {
            linkLabelLocation.Text = "Znajdujesz siê w pokoju eksperta";
        }

        public static Keys ConvertCharToVirtualKey(char ch)
        {
            short vkey = VkKeyScan(ch);
            Keys retval = (Keys)(vkey & 0xff);
            int modifiers = vkey >> 8;
            if ((modifiers & 1) != 0) retval |= Keys.Shift;
            if ((modifiers & 2) != 0) retval |= Keys.Control;
            if ((modifiers & 4) != 0) retval |= Keys.Alt;
            return retval;
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short VkKeyScan(char ch);

        void users_ToggleVideoTransmission(object sender, EventArgs e)
        {
            //if (ttclient.Flags.HasFlag(ClientFlag.CLIENT_VIDEOCAPTURE_READY))
            //{
                if (!transmitsVideo)
                {
                    transmitsVideo = true;
                    VideoCodec vidcodec = new VideoCodec();
                    vidcodec.nCodec = Codec.WEBM_VP8_CODEC;
                    vidcodec.webm_vp8.nRcTargetBitrate = 0;
                    ttclient.InitVideoCaptureDevice(settings.videoid, settings.capformat);
                    ttclient.StartVideoCaptureTransmission(vidcodec);
                }
                else
                {

                    transmitsVideo = false;
                    ttclient.StopVideoCaptureTransmission();
                    ttclient.CloseVideoCaptureDevice();
                }
            //}
            //else
            //{
            //    MessageBox.Show("Urz¹dzenie wideo nie jest skonfigurowane");
            //}
            UpdateControls();
        }

        void users_MessageUser(object sender, EventArgs e)
        {
            TeamTalkApp.NET.Utils.ChatParser.MessageEventArgs m = (TeamTalkApp.NET.Utils.ChatParser.MessageEventArgs)e;
            SendMessageToUser(Convert.ToInt32(m.Message));
        }

        #region AutoConnect
        private void ConnectToServer()
        {
            try
            {
                var server = commonSettings.Server;
                log.Info("Trying to connect to server: {0}, {1}, {2}, {3}", server.IP, server.TCPPort, server.UDPPort, server.Encrypted);
                if (!ttclient.Connect(server.IP, server.TCPPort, server.UDPPort, 0, 0, server.Encrypted))
                {
                    MessageBox.Show(String.Format("Nie uda³o po³¹czyæ siê z serwerem"),"B³¹d po³¹czenia",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    this.Close();
                    Application.Exit();
                    
                }
            } catch(Exception exc)
            {
                log.Error(exc.Message);
            }
            
       }

        void Log_OnConnectionSuccess()
        {
            AddStatusMessage("Connected to server");

            //the login dialog does its own comand error handling, so unregister event
            //ttclient.OnCmdError -= ttclient_OnCmdError;

            //LoginDlg dlg = new LoginDlg(ttclient, settings);
            //dlg.ShowDialog();

            //ttclient.OnCmdError += new TeamTalk.CommandError(ttclient_OnCmdError);                    

            var user = commonSettings.User;
            if (ttclient.DoLogin(user.Nick, user.Login, user.Password) < 0)
            {
                MessageBox.Show("Failed to issue client/server command");
                Application.Exit();
            }

            UpdateControls();
        }

        private void JoinChannel()
        {
            var channel = commonSettings.StartUPChannel;


            Channel chan = new Channel();
            ttclient.GetChannel(channel.ID, ref chan);

            AudioCodec codec = new AudioCodec();
            codec.nCodec = Codec.OPUS_CODEC;
            codec.opus.bDTX = true;
            codec.opus.bFEC = true;
            codec.opus.bVBR = true;
            codec.opus.bVBRConstraint = false;
            codec.opus.nApplication = 2048;
            codec.opus.nBitRate = 64000;
            codec.opus.nChannels = 1;
            codec.opus.nComplexity = 10;
            codec.opus.nSampleRate = 48000;
            codec.opus.nTxIntervalMSec = 40;

            codec.speex.nBandmode = 1;
            codec.speex.nQuality = 4;
            codec.speex.nTxIntervalMSec = 40;
            codec.speex.bStereoPlayback = false;
            chan.audiocodec = codec;
            ttclient.DoUpdateChannel(chan);

            SpeexDSP spxdsp = new SpeexDSP();
            ttclient.GetSoundInputPreprocess(ref spxdsp);
            spxdsp.nEchoSuppress = 0;
            spxdsp.nEchoSuppressActive = 0;
            spxdsp.nGainLevel = 0;
            spxdsp.nMaxDecDBSec = 0;
            spxdsp.nMaxGainDB = 0;
            spxdsp.nMaxIncDBSec = 0;
            spxdsp.nMaxNoiseSuppressDB = 0;
            spxdsp.bEnableEchoCancellation = false;
            spxdsp.bEnableDenoise = false;
            spxdsp.bEnableAGC = false;
            ttclient.SetSoundInputPreprocess(spxdsp);

            ////chan.audiocfg.bEnableAGC = true;
            ////chan.audiocfg.nGainLevel = 50;
            //ttclient.EnableVoiceTransmission(true);
            //ttclient.DoUpdateChannel(chan);

            InitInputOutputSound(ttclient);

            if (ttclient.DoJoinChannelByID(channel.ID, channel.Password) < 0)
            {
                MessageBox.Show("Failed to join channel");
                Application.Exit();
            };

        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {

            this.CenterToScreen();
            Utils.ChatParser.Instance.UpdateAgendaText += Instance_UpdateAgendaText;
            
        }

        void Instance_UpdateAgendaText(object sender, EventArgs e)
        {
            Utils.ChatParser.MessageEventArgs m = (Utils.ChatParser.MessageEventArgs)e;
            richTextBoxAgenda.Rtf = m.Message;
        }



        void UpdateControls()
        {
            ClientFlag flags = ttclient.Flags;
            UserType myusertype = ttclient.UserType;
            ServerProperties srvprop = new ServerProperties();

            if ((flags & ClientFlag.CLIENT_CONNECTED) != ClientFlag.CLIENT_CONNECTED)
                ResetControls();

            ttclient.GetServerProperties(ref srvprop);

            //int userid = channels.GetSelectedUser();
            //int channelid = channels.GetSelectedChannel();


            //We hide controls for editign agend if we are not admin
            if (myusertype == UserType.USERTYPE_DEFAULT)
            {
                richTextBoxAgenda.ReadOnly = true;
                toolStrip1.Visible = false;                

                richTextBoxAgenda.BorderStyle = BorderStyle.None;
                richTextBoxAgenda.BackColor = Color.White;

                nagrajDyskusjeToolStripMenuItem.Visible = false;
                nagrajDyskusjeToolStripMenuItem.Available = false;
                linkLabelGoToOrigin.Visible = false;
                linkLabelExpert.Visible = false;

                ustawieniaZaawansowaneToolStripMenuItem.Visible = false;
                ustawieniaZaawansowaneToolStripMenuItem.Available = false;


                miejsceNagraniaDyskusjiToolStripMenuItem.Visible = false;
                miejsceNagraniaDyskusjiToolStripMenuItem.Available = false;

                linkLabelSendRequest.Visible = true;
                linkLabelSendModeratorMessage.Visible = true;
                
            }
            else
            {
                //if (commonSettings.ModeratorChannel.ID == commonSettings.StartUPChannel.ID)
                //    linkLabelExpert.Visible = false;
                linkLabelSendRequest.Visible = false;
                linkLabelSendModeratorMessage.Visible = false;
            }

            connectToolStripMenuItem.Text =
                ((flags & ClientFlag.CLIENT_CONNECTED) == ClientFlag.CLIENT_CONNECTED ||
                (flags & ClientFlag.CLIENT_CONNECTING) == ClientFlag.CLIENT_CONNECTING) ? "Disconnect" : "Connect";

            changeNicknameToolStripMenuItem.Enabled = flags.HasFlag(ClientFlag.CLIENT_AUTHORIZED);
            changeStatusModeToolStripMenuItem.Enabled = flags.HasFlag(ClientFlag.CLIENT_AUTHORIZED);
            enableDesktopSharingToolStripMenuItem.Enabled = flags.HasFlag(ClientFlag.CLIENT_AUTHORIZED);
            enableDesktopSharingToolStripMenuItem.Checked = flags.HasFlag(ClientFlag.CLIENT_DESKTOP_ACTIVE);

            muteAllToolStripMenuItem.Enabled = flags.HasFlag(ClientFlag.CLIENT_SNDOUTPUT_READY);
            muteAllToolStripMenuItem.Checked = flags.HasFlag(ClientFlag.CLIENT_SNDOUTPUT_MUTE);
            storeAudioToDiskToolStripMenuItem.Enabled = flags.HasFlag(ClientFlag.CLIENT_AUTHORIZED);
            storeAudioToDiskToolStripMenuItem.Checked = settings.audiofolder.Length > 0;

        
            listUserAccountsToolStripMenuItem.Enabled = myusertype.HasFlag(UserType.USERTYPE_ADMIN);
            serverPropertiesToolStripMenuItem.Enabled = flags.HasFlag(ClientFlag.CLIENT_AUTHORIZED);
            saveConfigurationToolStripMenuItem.Enabled = myusertype.HasFlag(UserType.USERTYPE_ADMIN);
            broadcastTestMessageToolStripMenuItem.Enabled = flags.HasFlag(ClientFlag.CLIENT_AUTHORIZED);
            serverStatisticsToolStripMenuItem.Enabled = myusertype.HasFlag(UserType.USERTYPE_ADMIN);
        }

        void ResetControls()
        {
            userListFlowLayoutPanel.Controls.Clear();
        }

        void AddStatusMessage(string msg)
        {
            chatListBox.Items.Add("* " + msg + Environment.NewLine);
        }

        void ttclient_OnConnectionFailed()
        {
            AddStatusMessage("Failed to connect");

            //close connnection
            ttclient.Disconnect();
            UpdateControls();
        }

        void ttclient_OnConnectionLost()
        {
            AddStatusMessage("Connection lost");

            //close connnection
            ttclient.Disconnect();

            MessageBox.Show("Connection dropped");

            UpdateControls();
        }

        void ttclient_OnCmdProcessing(int nCmdID, bool bActive)
        {
            if (!bActive)
            {
                UpdateControls();
            }
        }

        void ttclient_OnCmdError(int nCmdID, ClientErrorMsg clienterrormsg)
        {
            MessageBox.Show(clienterrormsg.szErrorMsg, "Command Error");
        }

        void ttclient_OnCmdMyselfLoggedOut()
        {
            AddStatusMessage("Logged out of server");
        }

        void Join_Channel_OnCmdMyselfLoggedIn(int nMyUserID, UserAccount useraccount)
        {
            AddStatusMessage("Logged on to server successfully");
            string s = String.Format("User account information\r\n" +
                "Username: {0}\r\n" +
                "UserType: {1}\r\n" +
                "UserData: {2}\r\n" +
                "UserRights: {3}\r\n" +
                "Note: {4}\r\n" +
                "Initial Channel: {5}\r\n",
                useraccount.szUsername,
                useraccount.uUserType.ToString(),
                useraccount.nUserData,
                useraccount.uUserRights.ToString(),
                useraccount.szNote,
                useraccount.szInitChannel);
            AddStatusMessage(s);
            JoinChannel();
        }

        void ttclient_OnCmdUserLoggedIn(User user)
        {            
            //store audio to disk if an audio-folder has been specified
            if (!settings.muxed_audio_file && settings.audiofolder.Length > 0)
                ttclient.SetUserMediaStorageDir(user.nUserID, settings.audiofolder, "", settings.aff);
        }

        void ttclient_OnCmdUserJoinedChannel(User user)
        {
            //set default gain level for user (software gain volume)
            if (user.nChannelID == ttclient.GetMyChannelID())
                AddStatusMessage(user.szNickname + " joined channel");

            UpdateControls();
        }

        void ttclient_OnCmdUserLeftChannel(User user)
        {
            if (user.nChannelID == ttclient.GetMyChannelID())
                AddStatusMessage(user.szNickname + " left channel");
            UpdateControls();

            //if user has sent desktopinput ensure keys are released
            closeUserDesktopInput(user.nUserID);
        }

        void ttclient_OnCmdUserTextMessage(TextMessage textmessage)
        {
            switch (textmessage.nMsgType)
            {
                case TextMsgType.MSGTYPE_USER :
                    MessageDlg dlg;
                    if (msgdialogs.TryGetValue(textmessage.nFromUserID, out dlg))
                    {
                        dlg.NewMessage(textmessage);
                        if (!dlg.Visible)
                            dlg.Show();
                    }
                    else
                    {
                        dlg = new MessageDlg(ttclient, textmessage.nFromUserID);
                        dlg.FormClosed += new FormClosedEventHandler(dlg_MessageDlgClosed);
                        dlg.NewMessage(textmessage);
                        dlg.Show();
                        msgdialogs.Add(textmessage.nFromUserID, dlg);
                    }
                    break;
                case TextMsgType.MSGTYPE_CHANNEL:
                    {
                        User user = new User();
                        if (ttclient.GetUser(textmessage.nFromUserID, ref user))
                            chatListBox.Items.Add("<" + user.szNickname + "> " + textmessage.szMessage + Environment.NewLine);
                        Utils.ChatParser.Instance.Parse(textmessage.szMessage);
                        break;
                    }
                case TextMsgType.MSGTYPE_BROADCAST:
                    {
                        User user = new User();
                        if (ttclient.GetUser(textmessage.nFromUserID, ref user))
                        {
                            MessageBox.Show("Broadcast message from " + user.szNickname + Environment.NewLine +
                                            textmessage.szMessage);
                        }
                    }
                    break;
                case TextMsgType.MSGTYPE_CUSTOM:
                    {
                    }
                    break;
            }
        }

        void ttclient_OnCmdChannelNew(Channel channel)
        {
            UpdateControls();
        }

        void ttclient_OnCmdChannelUpdate(Channel channel)
        {
            UpdateControls();
        }

        void ttclient_OnCmdChannelRemove(Channel channel)
        {
            UpdateControls();
        }


        void ttclient_OnInternalError(ClientErrorMsg clienterrormsg)
        {
            MessageBox.Show(clienterrormsg.szErrorMsg, "Internal Error");
        }

        void ttclient_OnHotKeyToggle(int nHotKeyID, bool bActive)
        {
            switch ((HotKey)nHotKeyID)
            {
                case HotKey.HOTKEY_PUSHTOTALK :
                    bool speaking = ttclient.EnableVoiceTransmission(bActive);
                    users.VoiceEnabled = bActive && speaking;
                    speakingPictureBox.Visible = bActive && speaking;
                    pictureBoxSilent.Visible = !(bActive && speaking);
                    break;
            }
            Debug.WriteLine("HotKey " + nHotKeyID + " active " + bActive);
        }

        Dictionary<int, VideoFrame> videoframes = new Dictionary<int, VideoFrame>();

        void ttclient_OnUserVideoCapture(int nUserID, int nStreamID)
        {
            if (false) //from local capture device
            {
                //Release bitmap resources since we're about to release
                //the memory shared between the .NET application and the
                //TeamTalk DLL.
                if (local_bitmap != null)
                    local_bitmap.Dispose();

                VideoFrame vidfrm;

                //Release shared memory
                if (videoframes.TryGetValue(nUserID, out vidfrm))
                {
                    ttclient.ReleaseUserVideoCaptureFrame(vidfrm);
                    videoframes.Remove(nUserID);
                }

                Bitmap bmp;
                vidfrm = ttclient.AcquireUserVideoCaptureFrame(nUserID, out bmp);
                if (vidfrm.nFrameBufferSize>0)
                {
                }
                else
                {
                    //Failure situation. Set image to NULL so we don't 
                    //get an access violation exception by referencing
                    //released memory.=
                }
            }
            else
            {
                VideoDlg dlg;
                if (!viddialogs.TryGetValue(nUserID, out dlg))
                {
                    //Local video is 'nUserID' = 0;
                    dlg = new VideoDlg(ttclient, nUserID);
                    dlg.FormClosed += new FormClosedEventHandler(videodlg_FormClosed);
                    dlg.Show();
                    viddialogs.Add(nUserID, dlg);
              
                }
            }
        }

        void ttclient_OnUserRecordMediaFile(int nUserID, MediaFileInfo mediafileinfo)
        {
            User user = new User();
            ttclient.GetUser(nUserID, ref user);
            switch (mediafileinfo.nStatus)
            {
                case MediaFileStatus.MFS_STARTED :
                    AddStatusMessage("Start audio file for " + user.szNickname);
                    break;
                case MediaFileStatus.MFS_FINISHED:
                    AddStatusMessage("Finished audio file for " + user.szNickname);
                    break;
                case MediaFileStatus.MFS_ERROR:
                    AddStatusMessage("Error writing audio file for " + user.szNickname);
                    break;
                case MediaFileStatus.MFS_ABORTED:
                    AddStatusMessage("Aborted audio file for " + user.szNickname);
                    break;
            }
        }

        void ttclient_OnUserAudioBlock(int nUserID, StreamType nStreamType)
        {
            AudioBlock block = ttclient.AcquireUserAudioBlock(nStreamType, nUserID);
            if(block.nSamples>0)
            {
                ttclient.ReleaseUserAudioBlock(block);
            }
        }

        void ttclient_OnUserDesktopWindow(int nUserID, int nSessionID)
        {
            DesktopDlg dlg;
            if (!desktopdialogs.TryGetValue(nUserID, out dlg))
            {
                //Local video is 'nUserID' = 0;
                dlg = new DesktopDlg(ttclient, nUserID);
                desktopdialogs.Add(nUserID, dlg);
                dlg.FormClosed += new FormClosedEventHandler(desktopdlg_FormClosed);
                dlg.Show();
            }

            //release any keys held down by the user
            if (nSessionID == 0)
                closeUserDesktopInput(nUserID);
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClientFlag flags = ttclient.GetFlags();
            if (flags.HasFlag(ClientFlag.CLIENT_CONNECTED) ||
                flags.HasFlag(ClientFlag.CLIENT_CONNECTING))
            {
                ttclient.Disconnect();
                UpdateControls();
                return;
            }

            ConnectDlg dlg = new ConnectDlg(ttclient, settings);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if ((flags & ClientFlag.CLIENT_SNDINPUT_READY) != ClientFlag.CLIENT_SNDINPUT_READY &&
                    !ttclient.InitSoundInputDevice(settings.sndinputid))
                    MessageBox.Show("Failed to initialize sound input device");
                if ((flags & ClientFlag.CLIENT_SNDOUTPUT_READY) != ClientFlag.CLIENT_SNDOUTPUT_READY &&
                    !ttclient.InitSoundOutputDevice(settings.sndoutputid))
                    MessageBox.Show("Failed to initialize sound output device");
            }
            UpdateControls();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesDlg dlg = new PreferencesDlg(ttclient, settings);
            dlg.ShowDialog();
        }

        private void joinChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            int channelid = channels.GetSelectedChannel();
            if (channelid <= 0)
                return;
            //check if password protected
            Channel chan = new Channel();
            if (!ttclient.GetChannel(channelid, ref chan))
                return;
            string passwd = "";
            if (chan.bPassword)
                passwd = InputBox.Get("Join channel", "Password");
            ttclient.DoJoinChannelByID(channelid, passwd);
        }


        private void joinNewChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int channelid = channels.GetSelectedChannel();
            if (channelid == 0)
                channelid = ttclient.GetRootChannelID(); //make the root parent if nothing is selected

            ChannelDlg dlg = new ChannelDlg(ttclient, ChannelDlgType.JOIN_CHANNEL, 0, channelid);
            dlg.ShowDialog();
        }

       

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateControls();
        }

        private void chanmsgButton_Click(object sender, EventArgs e)
        {
            if (chanmsgTextBox.Text.Length > 0)
            {
                TextMessage msg;
                msg.nMsgType = TextMsgType.MSGTYPE_CHANNEL;
                msg.nChannelID = ttclient.GetMyChannelID();
                msg.nFromUserID = ttclient.GetMyUserID();
                msg.szFromUsername = ""; //not required
                msg.nToUserID = 0;
                msg.szMessage = chanmsgTextBox.Text;
                chanmsgTextBox.Text = "";
                ttclient.DoTextMessage(msg);
            }
        }

        private void enablePushToTalkToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void viewChannelInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int channelid = channels.GetSelectedChannel();
            ChannelDlg dlg = new ChannelDlg(ttclient, ChannelDlgType.VIEW_CHANNEL, channelid, 0);
            dlg.ShowDialog();
        }

        private void createChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int channelid = 0;
            if (channelid == 0)
                channelid = ttclient.GetRootChannelID(); //make the root parent if nothing is selected

            ChannelDlg dlg = new ChannelDlg(ttclient, ChannelDlgType.CREATE_CHANNEL, 0, channelid);
            dlg.ShowDialog();
        }

        private void updateChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int channelid = channels.GetSelectedChannel();
            ChannelDlg dlg = new ChannelDlg(ttclient, ChannelDlgType.UPDATE_CHANNEL, channelid, 0);
            dlg.ShowDialog();
        }

        private void deleteChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int channelid = channels.GetSelectedChannel();
            string name = "";
            ttclient.GetChannelPath(channelid, ref name);
            if (MessageBox.Show("Delete channel: " + name, "Delete Channel", MessageBoxButtons.YesNo) == DialogResult.Yes)
                ttclient.DoRemoveChannel(channelid);
        }

        private void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int userid = channels.GetSelectedUser();
            MessageDlg dlg;
            if (msgdialogs.TryGetValue(userid, out dlg))
                dlg.Show();
            else
            {
                dlg = new MessageDlg(ttclient, userid);
                dlg.FormClosed += new FormClosedEventHandler(dlg_MessageDlgClosed);
                dlg.Show();
                msgdialogs.Add(userid, dlg);
            }
        }

        void dlg_MessageDlgClosed(object sender, FormClosedEventArgs e)
        {
            foreach (KeyValuePair<int, MessageDlg> dlg in msgdialogs)
            {
                if (dlg.Value == sender)
                {
                    msgdialogs.Remove(dlg.Key);
                    break;
                }
            }
        }

        void videodlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (KeyValuePair<int, VideoDlg> dlg in viddialogs)
            {
                if (dlg.Value == sender)
                {
                    viddialogs.Remove(dlg.Key);
                    break;
                }
            }
        }

        void desktopdlg_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (KeyValuePair<int, DesktopDlg> dlg in desktopdialogs)
            {
                if (dlg.Value == sender)
                {
                    desktopdialogs.Remove(dlg.Key);
                    break;
                }
            }
        }

        private void changeNicknameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string nickname = InputBox.Get("Change Nickname", "Nickname");
            ttclient.DoChangeNickname(nickname);
        }

        private void changeStatusModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = InputBox.Get("Change Status Mode", "Status Message");
            ttclient.DoChangeStatus(0, msg);
        }

        private void opDeOpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User user = new User();
            if (!ttclient.GetUser(channels.GetSelectedUser(), ref user))
                return;

            ttclient.DoChannelOp(user.nUserID, user.nChannelID, 
                !ttclient.IsChannelOperator(user.nUserID, user.nChannelID));
        }

        private void kickToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User user = new User();
            if (!ttclient.GetUser(channels.GetSelectedUser(), ref user))
                return;
            //pass 0 as 'nChannelID' to kick from server instead of channel
            ttclient.DoKickUser(user.nUserID, user.nChannelID);
        }

        private void kickAndBanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User user = new User();
            if (!ttclient.GetUser(channels.GetSelectedUser(), ref user))
                return;
            //Req. UserRight.USERRIGHT_BAN_USERS
            //Req. UserRight.USERRIGHT_KICK_USERS
            ttclient.DoBanUser(user.nUserID, 0);
            ttclient.DoKickUser(user.nUserID, user.nChannelID);
        }

        private void listUserAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //only admins can do this.
            UserAccountsDlg dlg = new UserAccountsDlg(ttclient);
            dlg.ShowDialog();
        }

        private void serverPropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerPropertiesDlg dlg = new ServerPropertiesDlg(ttclient);
            dlg.ShowDialog();
        }

        private void saveConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ttclient.DoSaveConfig();
        }

        private void muteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ttclient.SetSoundOutputMute(muteAllToolStripMenuItem.Checked);
        }

        private void subscribeCommon(int userid, Subscription sub)
        {
            User user = new User();
            if (!ttclient.GetUser(userid, ref user))
                return;

            if ((user.uLocalSubscriptions & sub) == sub)
                ttclient.DoUnsubscribe(userid, sub);
            else
                ttclient.DoSubscribe(userid, sub);
        }

        private void userMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_USER_MSG);
        }

        private void channelMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_CHANNEL_MSG);
        }

        private void broadcastMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_BROADCAST_MSG);
        }

        private void voiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_VOICE);
        }

        private void videocaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_VIDEOCAPTURE);
        }

        private void desktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_DESKTOP);
        }

        private void desktopAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_DESKTOPINPUT);
        }

        private void allowDesktopAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            desktopAccessToolStripMenuItem_Click(sender, e);
        }

        private void interceptUserMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_INTERCEPT_USER_MSG);
        }

        private void interceptChannelMessagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_INTERCEPT_CHANNEL_MSG);
        }

        private void interceptvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_INTERCEPT_VOICE);
        }

        private void interceptvideocaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_INTERCEPT_VIDEOCAPTURE);
        }

        private void interceptDesktopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subscribeCommon(channels.GetSelectedUser(), Subscription.SUBSCRIBE_INTERCEPT_DESKTOP);
        }

        private void chanmsgTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                chanmsgButton_Click(sender, e);
        }

        private void viewUserInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserInfoDlg dlg = new UserInfoDlg(ttclient, channels.GetSelectedUser());
            dlg.ShowDialog();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ClientStatistics tmp = new ClientStatistics();
            if (!ttclient.GetClientStatistics(ref tmp))
                return;

            double totalrx = (tmp.nUdpBytesRecv - statistics.nUdpBytesRecv) / 1024;
            double totaltx = (tmp.nUdpBytesSent - statistics.nUdpBytesSent) / 1024;
            double voicerx = (tmp.nVoiceBytesRecv - statistics.nVoiceBytesRecv) / 1024;
            double voicetx = (tmp.nVoiceBytesSent - statistics.nVoiceBytesSent) / 1024;
            double videorx = (tmp.nVideoCaptureBytesRecv - statistics.nVideoCaptureBytesRecv) / 1024;
            double videotx = (tmp.nVideoCaptureBytesSent - statistics.nVideoCaptureBytesSent) / 1024;
            double desktoprx = (tmp.nDesktopBytesRecv - statistics.nDesktopBytesRecv) / 1024;
            double desktoptx = (tmp.nDesktopBytesSent - statistics.nDesktopBytesSent) / 1024;
            double mediafilerx = (tmp.nMediaFileAudioBytesRecv + tmp.nMediaFileVideoBytesRecv
                                  - statistics.nMediaFileAudioBytesRecv + statistics.nMediaFileVideoBytesRecv) / 1024;
            double mediafiletx = (tmp.nMediaFileAudioBytesSent + tmp.nMediaFileVideoBytesSent
                                  - statistics.nMediaFileAudioBytesSent + statistics.nMediaFileVideoBytesSent) / 1024;
            statistics = tmp;

            //toolStripStatusLabel1.Text = String.Format("RX: {0:F} TX: {1:F} VoiceRX: {2:F} VoiceTX {3:F} VideoRX {4:F} VideoTX {5:F}",
            //                totalrx, totaltx, voicerx, voicetx, videorx, videotx);

            //toolStripStatusLabel1.Text = String.Format("RX/TX Total: {0:F}/{1:F} Voice: {2:F}/{3:F} Video: {4:F}/{5:F} Desktop: {4:F}/{5:F} Media Files: {6:F}/{7:F} in KBytes",
                          //  totalrx, totaltx, voicerx, voicetx, videorx, videotx, desktoprx, desktoptx, mediafilerx, mediafiletx);
        }

        private void storeAudioToDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (storeAudioToDiskToolStripMenuItem.Checked)
            {
                AudioStorageDlg dlg = new AudioStorageDlg(settings);
                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                if (settings.muxed_audio_file)
                {
                    if (ttclient.ChannelID > 0)
                        newMuxedAudioRecording();
                }
                else
                {
                    //List<int> userids = users.GetUsers();
                    //foreach (int id in userids)
                    //    ttclient.SetUserMediaStorageDir(id, settings.audiofolder, "", settings.aff);
                    //To store audio in other channels, log in as admin, call DoSubscribe(.,SUBSCRIBE_INTERCEPT_AUDIO) 
                    //and set SetUserMediaStorageDir() on the user
                }
            }
            else
            {
                settings.audiofolder = "";
                settings.muxed_audio_file = false;
                settings.aff = AudioFileFormat.AFF_NONE;
                //clear if single file
                ttclient.StopRecordingMuxedAudioFile();
                //clear if separate files
                //List<int> userids = users.GetUsers();
                //foreach (int id in userids)
                //    ttclient.SetUserMediaStorageDir(id, "", "", settings.aff);
            }

            UpdateControls();
        }

        private void newMuxedAudioRecording()
        {
            

            Channel chan = new Channel();
            if (!ttclient.GetChannel(ttclient.ChannelID, ref chan))
            {
                MessageBox.Show("Must be in a channel to start muxed audio recording"); 
                return;
            }

            //generate a filename for the new mux file.
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("ja-JP");
            string timestamp = DateTime.Now.ToString();
            timestamp = timestamp.Replace(":", "");
            timestamp = timestamp.Replace("/", "");
            Thread.CurrentThread.CurrentCulture = currentCulture;
            string extension = ".mp3";
            switch (settings.aff)
            {
                case AudioFileFormat.AFF_MP3_64KBIT_FORMAT :
                case AudioFileFormat.AFF_MP3_128KBIT_FORMAT :
                case AudioFileFormat.AFF_MP3_256KBIT_FORMAT :
                    extension = ".mp3"; break;
                case AudioFileFormat.AFF_WAVE_FORMAT :
                    extension = ".wav";
                    break;
            }
            string dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (Properties.Settings.Default.recordDir != "none")
                    dir = Properties.Settings.Default.recordDir;
            string filename = dir + "\\" + timestamp.Replace(" ","_") + "_Conference" + extension;
            if (!ttclient.StartRecordingMuxedAudioFile(chan.audiocodec, filename, AudioFileFormat.AFF_MP3_128KBIT_FORMAT))
                MessageBox.Show("Failed to create muxed audio file: " + filename);
            else
                AddStatusMessage("Recording to " + filename);
        }

        List<ToolStripItem> contextmenuitems = new List<ToolStripItem>();
        ToolStripMenuItem menu;

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int userid = channels.GetSelectedUser();
            if (userid > 0)
            {
                menu = usersToolStripMenuItem;
                foreach(ToolStripItem item in usersToolStripMenuItem.DropDownItems)
                    contextmenuitems.Add(item);
            }
            else
            {
                menu = channelsToolStripMenuItem;
                foreach(ToolStripItem item in channelsToolStripMenuItem.DropDownItems)
                    contextmenuitems.Add(item);
            }
            foreach (ToolStripItem item in contextmenuitems)
                channelsContextMenuStrip.Items.Add(item);
            e.Cancel = false;
        }

        private void contextMenuStrip1_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            foreach (ToolStripItem item in contextmenuitems)
                menu.DropDownItems.Add(item);
            contextmenuitems.Clear();
        }

        private void broadcastTestMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TextMessage msg;
            msg.nMsgType = TextMsgType.MSGTYPE_BROADCAST;
            msg.nChannelID = 0;
            msg.nFromUserID = ttclient.GetMyUserID();
            msg.szFromUsername = ""; //not required
            msg.nToUserID = 0;
            msg.szMessage = InputBox.Get("Broadcast message", "Message");
            ttclient.DoTextMessage(msg);
        }

        private void filesContextMenuStrip_Opened(object sender, EventArgs e)
        {
            downloadFileToolStripMenuItem.Enabled = files.GetSelectedFile() > 0;
            uploadFileToolStripMenuItem.Enabled = files.GetSelectedChannel() > 0;
            deleteFileToolStripMenuItem.Enabled = files.GetSelectedFile() > 0;
        }

        private void uploadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ttclient.DoSendFile(files.GetSelectedChannel(), openFileDialog1.FileName);
            }
        }

        private void downloadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int fileid = files.GetSelectedFile();
            int channelid = files.GetSelectedChannel();

            RemoteFile file = new RemoteFile();
            if (!ttclient.GetChannelFile(channelid, fileid, ref file))
                return;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.FileName = file.szFileName;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                ttclient.DoRecvFile(channelid, fileid, saveFileDialog1.FileName);
        }

        private void deleteFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ttclient.DoDeleteFile(files.GetSelectedChannel(), files.GetSelectedFile());
        }

        void ttclient_OnFileTransfer(FileTransfer filetransfer)
        {
            if (filetransfer.nStatus == FileTransferStatus.FILETRANSFER_ACTIVE)
                new FileTransferDlg(ttclient, filetransfer.nTransferID).Show();
        }

      

       


        private void mutevoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User user = new User();
            if(ttclient.GetUser(channels.GetSelectedUser(), ref user))
                ttclient.SetUserMute(user.nUserID, StreamType.STREAMTYPE_VOICE, !user.uUserState.HasFlag(UserState.USERSTATE_MUTE_VOICE));
        }

        private void allowVoiceTransmissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User user = new User();
            Channel chan = new Channel();
            if (ttclient.GetUser(channels.GetSelectedUser(), ref user) && 
                ttclient.GetChannel(user.nChannelID, ref chan))
            {
                if(allowVoiceTransmissionToolStripMenuItem.Checked)
                {
                    if (chan.GetTransmitStreamTypes(user.nUserID) == StreamType.STREAMTYPE_NONE &&
                        chan.GetTransmitUserCount() >= TeamTalk.TT_TRANSMITUSERS_MAX)
                        MessageBox.Show("Maximum users to transmit is " + TeamTalk.TT_TRANSMITUSERS_MAX.ToString());
                    else
                        chan.AddTransmitUser(user.nUserID, StreamType.STREAMTYPE_VOICE);
                }
                else
                    chan.RemoveTransmitUser(user.nUserID, StreamType.STREAMTYPE_VOICE);

                ttclient.DoUpdateChannel(chan);
            }
        }

        private void allowVideoTransmissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User user = new User();
            Channel chan = new Channel();
            if (ttclient.GetUser(channels.GetSelectedUser(), ref user) &&
                ttclient.GetChannel(user.nChannelID, ref chan))
            {
                if (allowVideoTransmissionToolStripMenuItem.Checked)
                {
                    if (chan.GetTransmitStreamTypes(user.nUserID) == StreamType.STREAMTYPE_NONE &&
                        chan.GetTransmitUserCount() >= TeamTalk.TT_TRANSMITUSERS_MAX)
                        MessageBox.Show("Maximum users to transmit is " + TeamTalk.TT_TRANSMITUSERS_MAX.ToString());
                    else
                        chan.AddTransmitUser(user.nUserID, StreamType.STREAMTYPE_VIDEOCAPTURE);
                }
                else
                    chan.RemoveTransmitUser(user.nUserID, StreamType.STREAMTYPE_VIDEOCAPTURE);

                ttclient.DoUpdateChannel(chan);
            }
        }

        private void serverStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ServerStatsDlg(ttclient).ShowDialog();
        }

     

       

       

        private void enableDesktopSharingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ttclient.Flags.HasFlag(ClientFlag.CLIENT_DESKTOP_ACTIVE))
            {
                ttclient.CloseDesktopWindow();
                UpdateControls();

                senddesktopTimer.Enabled = false;
                sendcursorTimer.Enabled = false;
                return;
            }

            DesktopShareDlg dlg = new DesktopShareDlg();
            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            //'hShareWnd' == IntPtr.Zero means share active window
            hShareWnd = dlg.hShareWnd;
            bmpShareFormat = dlg.bmpformat;

            if (sendDesktopWindow())
            {
                if (dlg.update_interval > 0)
                {
                    senddesktopTimer.Interval = dlg.update_interval;
                    senddesktopTimer.Enabled = true;
                }
                //send desktop cursor every 50 msec
                if (dlg.share_cursor)
                {
                    sendcursorTimer.Interval = 50;
                    sendcursorTimer.Enabled = true;
                }
            }
            else
                MessageBox.Show("Failed to send shared window", "Desktop Sharing Error");

            UpdateControls();
        }

        bool sendDesktopWindow()
        {
            //figure out which window to share
            IntPtr hWnd;
            if (hShareWnd == IntPtr.Zero)
                hWnd = WindowsHelper.GetDesktopActiveHWND();
            else
                hWnd = hShareWnd;

            //don't try to send new bitmap if one is already being transmitted
            if ((ttclient.Flags & ClientFlag.CLIENT_TX_DESKTOP) == ClientFlag.CLIENT_CLOSED)
                return ttclient.SendDesktopWindowFromHWND(hWnd, bmpShareFormat,
                                                          DesktopProtocol.DESKTOPPROTOCOL_ZLIB_1)>0;
            return false;
        }

        private void senddesktopTimer_Tick(object sender, EventArgs e)
        {
            sendDesktopWindow();
        }

        private void sendcursorTimer_Tick(object sender, EventArgs e)
        {
            ttclient.SendDesktopCursorPosition((ushort)Cursor.Position.X, (ushort)Cursor.Position.Y);
        }

        private void streamMediaFileToChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((ttclient.Flags & (ClientFlag.CLIENT_STREAM_AUDIO | ClientFlag.CLIENT_STREAM_VIDEO)) != ClientFlag.CLIENT_CLOSED)
            {
                ttclient.StopStreamingMediaFileToChannel();
                streamMediaFileToChannelToolStripMenuItem.Checked = false;
            }
            else
                new MediaFileDlg(ttclient).ShowDialog();
        }

        void ttclient_OnStreamMediaFile(MediaFileInfo mediafileinfo)
        {
            switch (mediafileinfo.nStatus)
            {
                case MediaFileStatus.MFS_ABORTED:
                    AddStatusMessage("Aborted media file stream to channel");
                    break;
                case MediaFileStatus.MFS_ERROR:
                    AddStatusMessage("Error in media file stream to channel");
                    break;
                case MediaFileStatus.MFS_FINISHED:
                    AddStatusMessage("Finished media file stream to channel");
                    break;
                case MediaFileStatus.MFS_STARTED:
                    AddStatusMessage("Started media file stream to channel");
                    break;
            }
            streamMediaFileToChannelToolStripMenuItem.Checked = mediafileinfo.nStatus == MediaFileStatus.MFS_STARTED;
        }

        

        //container of users' past key-down events
        // userid -> [key-code, DesktopInput]
        Dictionary<int, Dictionary<uint, DesktopInput>> desktopinputs = new Dictionary<int, Dictionary<uint, DesktopInput>>();

        void ttclient_OnUserDesktopInput(int nSrcUserID, DesktopInput desktopinput)
        {
            DesktopInput[] inputs = new DesktopInput[] { desktopinput }, trans_inputs = null;
            //assumes desktop input is received in TTKEYCODE format
            WindowsHelper.DesktopInputKeyTranslate(TTKeyTranslate.TTKEY_TTKEYCODE_TO_WINKEYCODE,
                                       inputs, out trans_inputs);
            WindowsHelper.DesktopInputExecute(trans_inputs);

            //create new (or find existing) list of desktop inputs from user
            Dictionary<uint, DesktopInput> pastinputs;
            if (!desktopinputs.TryGetValue(nSrcUserID, out pastinputs))
            {
                pastinputs = new Dictionary<uint, DesktopInput>();
                desktopinputs.Add(nSrcUserID, pastinputs);
            }

            //only store key-down events and remove previous key-down events if 
            //the keys have been released
            foreach (DesktopInput input in trans_inputs)
            {
                if (input.uKeyState == DesktopKeyState.DESKTOPKEYSTATE_DOWN)
                    pastinputs.Add(input.uKeyCode, input);
                else if (input.uKeyState == DesktopKeyState.DESKTOPKEYSTATE_UP)
                    pastinputs.Remove(input.uKeyCode);
            }

            //if no keys are held by user then remove the user
            if (pastinputs.Count == 0)
                desktopinputs.Remove(nSrcUserID);
        }

        //release keys which have been held down by user
        void closeUserDesktopInput(int nUserID)
        {
            Dictionary<uint, DesktopInput> pastinputs;
            if (!desktopinputs.TryGetValue(nUserID, out pastinputs))
                return;

            DesktopInput[] inputs = new DesktopInput[pastinputs.Count];
            int i=0;
            foreach (KeyValuePair<uint, DesktopInput> pair in pastinputs)
            {
                //invert key-down event so it's now a key up event
                //(and all keys are release by the user)
                DesktopInput input = pair.Value;
                input.uKeyState = DesktopKeyState.DESKTOPKEYSTATE_UP;
                inputs[i++] = input;
            }
            WindowsHelper.DesktopInputExecute(inputs);
        }

        private void tempConnectButton_Click(object sender, EventArgs e)
        {
            ClientFlag flags = ttclient.GetFlags();
            if (flags.HasFlag(ClientFlag.CLIENT_CONNECTED) ||
                flags.HasFlag(ClientFlag.CLIENT_CONNECTING))
            {
                ttclient.Disconnect();
                UpdateControls();
                return;
            }

            ConnectDlg dlg = new ConnectDlg(ttclient, settings);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                if ((flags & ClientFlag.CLIENT_SNDINPUT_READY) != ClientFlag.CLIENT_SNDINPUT_READY &&
                    !ttclient.InitSoundInputDevice(settings.sndinputid))
                    MessageBox.Show("Failed to initialize sound input device");
                if ((flags & ClientFlag.CLIENT_SNDOUTPUT_READY) != ClientFlag.CLIENT_SNDOUTPUT_READY &&
                    !ttclient.InitSoundOutputDevice(settings.sndoutputid))
                    MessageBox.Show("Failed to initialize sound output device");
            }
            UpdateControls();
        }

        private void speakButton_Click(object sender, EventArgs e)
        {

        }

        private void requestButton_Click(object sender, EventArgs e)
        {
            SendSpeakRequestToModerator();
        }

        private void SendSpeakRequestToModerator()
        {
            TextMessage msg;
            msg.nMsgType = TextMsgType.MSGTYPE_CHANNEL;
            msg.nChannelID = ttclient.GetMyChannelID();
            msg.nFromUserID = ttclient.GetMyUserID();
            msg.szFromUsername = ""; //not required
            msg.nToUserID = 0;
            msg.szMessage = String.Format("QUEUE:{0}", ttclient.UserID);
            ttclient.DoTextMessage(msg);
        }

        private void MainWindow_KeyPress(object sender, KeyPressEventArgs e)
        {
        
        }

        private void toolStripButtonRTFBold_Click(object sender, EventArgs e)
        {
            if (richTextBoxAgenda.SelectionFont.Bold)
                richTextBoxAgenda.SelectionFont = new Font(richTextBoxAgenda.SelectionFont, FontStyle.Regular);
            else
                richTextBoxAgenda.SelectionFont = new Font(richTextBoxAgenda.SelectionFont, FontStyle.Bold);
        }

        private void toolStripButtonRTFItalics_Click(object sender, EventArgs e)
        {
            if (richTextBoxAgenda.SelectionFont.Italic)
                richTextBoxAgenda.SelectionFont = new Font(richTextBoxAgenda.SelectionFont, FontStyle.Regular);
            else
                richTextBoxAgenda.SelectionFont = new Font(richTextBoxAgenda.SelectionFont, FontStyle.Italic);
        }

        private void toolStripButtonRTFStroke_Click(object sender, EventArgs e)
        {
            if (richTextBoxAgenda.SelectionFont.Strikeout)
                richTextBoxAgenda.SelectionFont = new Font(richTextBoxAgenda.SelectionFont, FontStyle.Regular);
            else
             richTextBoxAgenda.SelectionFont = new Font(richTextBoxAgenda.SelectionFont, FontStyle.Strikeout);
        }

        private void toolStripButtonRTFUnderline_Click(object sender, EventArgs e)
        {

            if (richTextBoxAgenda.SelectionFont.Underline)
                richTextBoxAgenda.SelectionFont = new Font(richTextBoxAgenda.SelectionFont, FontStyle.Regular);
            else
                richTextBoxAgenda.SelectionFont = new Font(richTextBoxAgenda.SelectionFont, FontStyle.Underline);
        }

        private void toolStripButtonRTFNormal_Click(object sender, EventArgs e)
        {
            richTextBoxAgenda.SelectionFont = new Font(richTextBoxAgenda.SelectionFont, FontStyle.Regular);
        }

        private void toolStripButtonAgendaSend_Click(object sender, EventArgs e)
        {
            users.Agenda = richTextBoxAgenda.Rtf;
            users.BroadcastAgenda();
        }

        private void toolStripButtonList_Click(object sender, EventArgs e)
        {
            richTextBoxAgenda.SelectionBullet = !richTextBoxAgenda.SelectionBullet;
        }

        private void toolStripButtonColor_Click(object sender, EventArgs e)
        {
            colorDialogAgenda.Color = richTextBoxAgenda.SelectionColor;
            if (colorDialogAgenda.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                richTextBoxAgenda.SelectionColor = colorDialogAgenda.Color;
            }
        }

        private void toolStripButtonFont_Click(object sender, EventArgs e)
        {
            fontDialogAgenda.Font = richTextBoxAgenda.SelectionFont;
            if (fontDialogAgenda.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                richTextBoxAgenda.SelectionFont = fontDialogAgenda.Font;
            }  
        }

        private void informacjeDlaOsóbNiewidomychToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.AccessibilityInformationWindow dialog = new Forms.AccessibilityInformationWindow();
            dialog.ShowDialog();
        }

        private void oProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.AboutWindow dialog = new Forms.AboutWindow();
            dialog.ShowDialog();
        }

        private void zglosWypowiedziToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendSpeakRequestToModerator();
        }

        private void nagrajDyskusjeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Recording)
            {
                newMuxedAudioRecording();
                Recording = true;
                labelRecord.Visible = Recording;
                nagrajDyskusjeToolStripMenuItem.Checked = true;
            }
            else
            {
                Recording = false;
                ttclient.StopRecordingMuxedAudioFile();
                labelRecord.Visible = Recording;
                
                nagrajDyskusjeToolStripMenuItem.Checked = false;
            }

        }

        private void userListFlowLayoutPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SendSpeakRequestToModerator();
        }

        private void linkLabelSendModeratorMessage_Click(object sender, EventArgs e)
        {
            SendMessageToModerator();
        }

        private void SendMessageToModerator()
        {
            int userid = users.currentChannelModerator;
            MessageDlg dlg;
            if (msgdialogs.TryGetValue(userid, out dlg))
                dlg.Show();
            else
            {
                dlg = new MessageDlg(ttclient, userid);
                dlg.FormClosed += new FormClosedEventHandler(dlg_MessageDlgClosed);
                dlg.Show();
                msgdialogs.Add(userid, dlg);
            }
        }

        private void SendMessageToUser(int userid)
        {
            MessageDlg dlg;
            if (msgdialogs.TryGetValue(userid, out dlg))
                dlg.Show();
            else
            {
                dlg = new MessageDlg(ttclient, userid);
                dlg.FormClosed += new FormClosedEventHandler(dlg_MessageDlgClosed);
                dlg.Show();
                msgdialogs.Add(userid, dlg);
            }
        }

        private void linkLabelSendModeratorMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SendMessageToModerator();
        }

        private void linkLabelExpert_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TextMessage msg;
            msg.nMsgType = TextMsgType.MSGTYPE_CHANNEL;
            msg.nChannelID = ttclient.GetMyChannelID();
            msg.nFromUserID = ttclient.GetMyUserID();
            msg.szFromUsername = ""; //not required
            msg.nToUserID = 0;
            msg.szMessage = String.Format("MOVETO:EXPERT");
            ttclient.DoTextMessage(msg);
            linkLabelExpert.Visible = false;
            linkLabelGoToOrigin.Visible = true;
        }

        private void linkLabelGoToOrigin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TextMessage msg;
            msg.nMsgType = TextMsgType.MSGTYPE_CHANNEL;
            msg.nChannelID = ttclient.GetMyChannelID();
            msg.nFromUserID = ttclient.GetMyUserID();
            msg.szFromUsername = ""; //not required
            msg.nToUserID = 0;
            msg.szMessage = String.Format("MOVETO:ORIGIN");
            ttclient.DoTextMessage(msg);
            linkLabelExpert.Visible = true;
            linkLabelGoToOrigin.Visible = false;
        }

        private void ustawieniaKlawiszyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.HotKeys.Clear();

            Properties.Settings.Default.Save();
            HotKeyDlg dlg = new HotKeyDlg(ttclient);
            dlg.ShowDialog();
            Properties.Settings.Default.HotKeySet = true;
            Properties.Settings.Default.Save();

            List<int> keys = new List<int>();
            StringBuilder keyString = new StringBuilder();
            foreach (String key in Properties.Settings.Default.HotKeys)
            {
                keys.Add(Convert.ToInt32(key));
                if (key == "162")
                    keyString.Append("Ctrl");
                else if (key == "1")
                    keyString.Append("LPM");
                else if (key == "2")
                    keyString.Append("PPM");
                else if (key == "4")
                    keyString.Append("ŒPM");
                else if (key == "160")
                    keyString.Append("Shift");
                else if (key == "164")
                    keyString.Append("Alt");
                else
                    keyString.Append((char)Convert.ToInt32(key));
                keyString.Append("+");
            }
            keyString.Remove(keyString.Length - 1, 1);

            ttclient.HotKey_Unregister((int)HotKey.HOTKEY_PUSHTOTALK);
            ttclient.HotKey_Register((int)HotKey.HOTKEY_PUSHTOTALK, keys.ToArray());


            linkLabelSpeak.Text = String.Format("Aby mówiæ, wciœnij {0}", keyString.ToString());

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ttclient.Disconnect();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            ConnectToServer();
            UpdateControls();
        }

        public bool transmitsVideo { get; set; }

        private void ustawieniaZaawansowaneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PreferencesDlg prefs = new PreferencesDlg(ttclient,settings);
            prefs.ShowDialog();
        }

        private void napiszDoModeratoraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendMessageToModerator();
        }

        private bool checkSettings()
        {
           // return Properties.Settings.Default.AudioInLevel > 0 && Properties.Settings.Default.AudioOutLevel > 0
             //   && Properties.Settings.Default.AudioSaved;
            return false;
        }

        public bool a { get; set; }

        public bool bb { get; set; }

        public bool c { get; set; }

        public int dd { get; set; }

        public int ee { get; set; }

        public bool ss { get; set; }

        //private bool InitInputOutputSound(TeamTalk ttclient)
        //{
        //    int devin = 0, devout = 0;

        //    bool defsnddev = TeamTalk.GetDefaultSoundDevicesEx(SoundSystem.SOUNDSYSTEM_WASAPI, ref devin, ref devout);

        //    SpeexDSP spxdsp = new SpeexDSP();
        //    ttclient.GetSoundInputPreprocess(ref spxdsp);
        //    spxdsp.nEchoSuppress = 0;
        //    spxdsp.nEchoSuppressActive = 0;
        //    spxdsp.nGainLevel = 0;
        //    spxdsp.nMaxDecDBSec = 0;
        //    spxdsp.nMaxGainDB = 0;
        //    spxdsp.nMaxIncDBSec = 0;
        //    spxdsp.nMaxNoiseSuppressDB = 0;
        //    ttclient.SetSoundInputPreprocess(spxdsp);

        //    bool isid = ttclient.InitSoundInputDevice(devin);
        //    bool ttfhci = ttclient.Flags.HasFlag(ClientFlag.CLIENT_SNDINPUT_READY);
        //    bool isod = ttclient.InitSoundOutputDevice(devout);
        //    bool ttfhco = ttclient.Flags.HasFlag(ClientFlag.CLIENT_SNDOUTPUT_READY);

        //    return defsnddev && isid && ttfhci;
        //}

        private bool InitInputOutputSound(TeamTalk ttclient)
        {
            int devin = 0, devout = 0;

            bool defsnddev = TeamTalk.GetDefaultSoundDevicesEx(SoundSystem.SOUNDSYSTEM_WASAPI, ref devin, ref devout);

            SpeexDSP spxdsp = new SpeexDSP();
            ttclient.GetSoundInputPreprocess(ref spxdsp);
            spxdsp.bEnableAGC = true;
            spxdsp.bEnableEchoCancellation = true;
            spxdsp.bEnableDenoise = true;


            spxdsp.nGainLevel = Properties.Settings.Default.AudioInLevel;
            
            //spxdsp.nGainLevel = SpeexDSPConstants.DEFAULT_AGC_GAINLEVEL;
            spxdsp.nMaxIncDBSec = SpeexDSPConstants.DEFAULT_AGC_INC_MAXDB;
            spxdsp.nMaxDecDBSec = SpeexDSPConstants.DEFAULT_AGC_DEC_MAXDB;
            spxdsp.nMaxGainDB = SpeexDSPConstants.DEFAULT_AGC_GAINMAXDB;

            spxdsp.nMaxNoiseSuppressDB = SpeexDSPConstants.DEFAULT_DENOISE_SUPPRESS;

            spxdsp.nEchoSuppress = SpeexDSPConstants.DEFAULT_ECHO_SUPPRESS;
            spxdsp.nEchoSuppressActive = SpeexDSPConstants.DEFAULT_ECHO_SUPPRESS_ACTIVE;


            //spxdsp.nEchoSuppress = 0;
            //spxdsp.nEchoSuppressActive = 0;
            //spxdsp.nGainLevel = 0;
            //spxdsp.nMaxDecDBSec = 0;
            //spxdsp.nMaxGainDB = 0;
            //spxdsp.nMaxIncDBSec = 0;
            //spxdsp.nMaxNoiseSuppressDB = 0;

            ttclient.SetSoundInputPreprocess(spxdsp);
            //ttclient.EnableVoiceTransmission(true);
            ttclient.SetVoiceActivationLevel(20);
           
            bool isid = ttclient.InitSoundInputDevice(devin);
            bool ttfhci = ttclient.Flags.HasFlag(ClientFlag.CLIENT_SNDINPUT_READY);
            bool isod = ttclient.InitSoundOutputDevice(devout);
            bool ttfhco = ttclient.Flags.HasFlag(ClientFlag.CLIENT_SNDOUTPUT_READY);

            return defsnddev && isid && ttfhci;
        }


        public bool Recording { get; set; }

        private void linkLabelSpeak_MouseDown(object sender, MouseEventArgs e)
        {
            bool bActive = true;
            bool speaking = ttclient.EnableVoiceTransmission(bActive);
            users.VoiceEnabled = bActive && speaking;
            speakingPictureBox.Visible = bActive && speaking;
            pictureBoxSilent.Visible = !(bActive && speaking);
        }

        private void linkLabelSpeak_MouseUp(object sender, MouseEventArgs e)
        {
            bool bActive = false;
            bool speaking = ttclient.EnableVoiceTransmission(bActive);
            users.VoiceEnabled = bActive && speaking;
            speakingPictureBox.Visible = bActive && speaking;
            pictureBoxSilent.Visible = !(bActive && speaking);
        }

        private void ustawieniaDŸwiêkuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Forms.AutoMicroConf auto = new Forms.AutoMicroConf(ttclient, commonSettings, settings);
            auto.ShowDialog();

            SpeexDSP spxdsp = new SpeexDSP(true);
            spxdsp.bEnableAGC = Properties.Settings.Default.bEnableAGC;
            spxdsp.bEnableDenoise = Properties.Settings.Default.bEnableDenoise;
            spxdsp.bEnableEchoCancellation = Properties.Settings.Default.bEnableEchoCancellation;
            ttclient.SetSoundInputPreprocess(spxdsp);
            //ttclient.SetSoundOutputVolume(24000);
            ttclient.SetSoundInputGainLevel(Properties.Settings.Default.AudioInLevel);
            if (!InitInputOutputSound(ttclient))
            {
                MessageBox.Show("Problem z inicjalizacj¹ ustawieñ urz¹dzeñ wejœcia i wyjœcia");
            }
        }

        private bool voiceActivationEnabled = false;

        private void aktywacjaG³osowaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            voiceActivationEnabled = !voiceActivationEnabled;
            aktywacjaG³osowaToolStripMenuItem.Checked = voiceActivationEnabled;
            ttclient.EnableVoiceActivation(voiceActivationEnabled);
        }

        private void linkLabelSpeak_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void miejsceNagraniaDyskusjiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.recordDir = folderBrowserDialog1.SelectedPath;
                Properties.Settings.Default.Save();

            }
        }


    }


}
