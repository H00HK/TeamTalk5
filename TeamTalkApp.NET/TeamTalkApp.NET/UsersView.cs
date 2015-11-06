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
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using BearWare;
using Newtonsoft.Json;
using TeamTalkLib;
using TeamTalkLib.Settings;
using TeamTalkLib.Storage;
using TeamTalkLib.NET.Recording;

namespace TeamTalkApp.NET
{
    class UsersView
    {

        private UserManager userManager;

        protected TeamTalk ttclient;
        public String Agenda;
        FlowLayoutPanel listview;
        Dictionary<String, UserControls.UserLabel> userLabels;
        Dictionary<String, int> userQueue;
        Dictionary<String, StreamType> userStreamType;
        ConnectionSettings settings;

        public event EventHandler MessageUser;
        public event EventHandler ToggleVideoTransmission;
        public event EventHandler InOriginChannel;
        public event EventHandler InExpertChannel;

        public int currentChannelModerator { get; set; }

        public UsersView(TeamTalk tt, FlowLayoutPanel list, ConnectionSettings settings)
        {
            voiceEnabled = false;
            this.settings = settings;
            ttclient = tt;
            Agenda = String.Empty;
            ttclient.OnCmdUserLoggedIn += new TeamTalk.UserUpdate(ttclient_OnCmdUserLoggedIn);
            ttclient.OnCmdUserLoggedOut += new TeamTalk.UserUpdate(ttclient_OnCmdUserLoggedOut);
            ttclient.OnCmdUserUpdate += new TeamTalk.UserUpdate(ttclient_OnCmdUserUpdate);

            ttclient.OnCmdChannelUpdate += new TeamTalk.ChannelUpdate(ttclient_OnCmdChannelUpdate);

            ttclient.OnCmdUserJoinedChannel += new TeamTalk.UserUpdate(ttclient_OnCmdUserJoinedChannel);
            ttclient.OnCmdUserLeftChannel += new TeamTalk.UserUpdate(ttclient_OnCmdUserLeftChannel);


            ttclient.OnUserStateChange += new TeamTalk.UserUpdate(ttclient_OnUserStateChange);
            ttclient.OnVoiceActivation += new TeamTalk.VoiceActivation(ttclient_OnVoiceActivation);
            userLabels = new Dictionary<string, UserControls.UserLabel>();
            listview = list;

            userQueue = new Dictionary<string, int>();
            userStreamType = new Dictionary<String, StreamType>();      

            Utils.ChatParser.Instance.UpdateQueue += Instance_UpdateQueue;
            Utils.ChatParser.Instance.UpdateUserList += Instance_UpdateUserList;
            Utils.ChatParser.Instance.MoveToExpert += Instance_MoveToExpert;
            Utils.ChatParser.Instance.MoveToOrigin += Instance_MoveToOrigin;
            Utils.ChatParser.Instance.UpdateQueueList += Instance_UpdateQueueList;
            Utils.ChatParser.Instance.UpdateStreamList += Instance_UpdateStreamList;

        }

        ~UsersView()
        {
            if ((userManager != null))
            {
                userManager.StoreUserStatistics();
            }
            
        }

        void Instance_MoveToOrigin(object sender, EventArgs e)
        {
            //ttclient.DoLeaveChannel();
            if (settings.StartUPChannel.ID != settings.ModeratorChannel.ID)
                ttclient.DoJoinChannelByID(settings.StartUPChannel.ID, settings.StartUPChannel.Password);
        }

        void Instance_MoveToExpert(object sender, EventArgs e)
        {
            //ttclient.DoLeaveChannel();
            if (settings.StartUPChannel.ID != settings.ModeratorChannel.ID)
            {
                ChannelSettings modChannel = settings.ModeratorChannel;
                ttclient.DoJoinChannelByID(modChannel.ID, modChannel.Password);
            }
        }

        void Instance_UpdateStreamList(object sender, EventArgs e)
        {
            Utils.ChatParser.MessageEventArgs m = (Utils.ChatParser.MessageEventArgs)e;
            userStreamType = JsonConvert.DeserializeObject<Dictionary<String, StreamType>>(m.Message);

            if (ttclient.UserType.HasFlag(UserType.USERTYPE_ADMIN))
            {
                Channel chan = new Channel();
                User user = new User();
                if (ttclient.GetUser(ttclient.UserID, ref user) &&
                    ttclient.GetChannel(user.nChannelID, ref chan))
                {

                    ttclient.DoUpdateChannel(chan);
                    foreach (KeyValuePair<String, StreamType> userStream in userStreamType)
                    {
                        chan.AddTransmitUser(int.Parse(userStream.Key), userStream.Value);
                    }
                    ttclient.DoUpdateChannel(chan);
                }
            }
            UpdateUIFromUsers();
            
        }

    

        void Instance_UpdateQueue(object sender, EventArgs e)
        {
            Utils.ChatParser.MessageEventArgs m = (Utils.ChatParser.MessageEventArgs)e;
            if (!userQueue.ContainsKey(m.Message))
            {
                userQueue.Add(m.Message, userQueue.Count);
                BroadcastQueueList();
            }    
        }

        private void BroadcastQueueList()
        {
            if (ttclient.UserType == UserType.USERTYPE_ADMIN)
            {
                TextMessage msg;
                msg.nMsgType = TextMsgType.MSGTYPE_CHANNEL;
                msg.nChannelID = ttclient.GetMyChannelID();
                msg.nFromUserID = ttclient.GetMyUserID();
                msg.szFromUsername = "";
                msg.nToUserID = 0;
                msg.szMessage = String.Format("QUEUE_LIST:{0}", JsonConvert.SerializeObject(userQueue));
                ttclient.DoTextMessage(msg);
            }
        }

        public void BroadcastAgenda()
        {
            if (ttclient.UserType == UserType.USERTYPE_ADMIN)
            {
                TextMessage msg;
                msg.nMsgType = TextMsgType.MSGTYPE_CHANNEL;
                msg.nChannelID = ttclient.GetMyChannelID();
                msg.nFromUserID = ttclient.GetMyUserID();
                msg.szFromUsername = ""; //not required
                msg.nToUserID = 0;

                msg.szMessage = String.Format("AGENDA:{0}", Utils.StringCompression.Compress(@Agenda));
                ttclient.DoTextMessage(msg);
            }
        }

        private void BroadcastUserStreamTypeList()
        {
            if (!userStreamType.ContainsKey(ttclient.UserID.ToString()))
            {
                userStreamType.Add(ttclient.UserID.ToString(), StreamType.STREAMTYPE_VOICE);
            }
            if (ttclient.UserType == UserType.USERTYPE_ADMIN)
            {
                TextMessage msg;
                msg.nMsgType = TextMsgType.MSGTYPE_CHANNEL;
                msg.nChannelID = ttclient.GetMyChannelID();
                msg.nFromUserID = ttclient.GetMyUserID();
                msg.szFromUsername = "";
                msg.nToUserID = 0;
                msg.szMessage = String.Format("STREAM_LIST:{0}", JsonConvert.SerializeObject(userStreamType));
                ttclient.DoTextMessage(msg);
            }
        }

        void Instance_UpdateQueueList(object sender, EventArgs e)
        {
            Utils.ChatParser.MessageEventArgs m = (Utils.ChatParser.MessageEventArgs)e;
            userQueue = JsonConvert.DeserializeObject<Dictionary<String, int>>(m.Message);
            int queuePosition = 0;
            List<string> keys = new List<string>(userQueue.Keys);
            foreach(string key in keys)
            {
                userQueue[key] = queuePosition;
                queuePosition++;
            }
            UpdateUIFromUsers();
        }

        void Instance_UpdateUserList(object sender, EventArgs e)
        {
            //TODO: Update all users

            UpdateUIFromUsers();
        }

        void ttclient_OnUserStateChange(User user)
        {
            UserControls.UserLabel item = userLabels[user.nUserID.ToString()];

            if (item != null)
            {

                bool speaking = user.uUserState.HasFlag(UserState.USERSTATE_VOICE);
                item.Speaking = speaking;                                
            }
            UpdateUIFromUsers();
        }

        public void ttclient_OnVoiceActivation(bool bVoiceActive)
        {
            UserControls.UserLabel item = userLabels[ttclient.GetMyUserID().ToString()];
            
            if (item != null)
            {
                if (bVoiceActive || ttclient.Flags.HasFlag(ClientFlag.CLIENT_TX_VOICE))
                    item.Speaking = true;
                else
                    item.Speaking = false;
            }
            UpdateUIFromUsers();
        }

        void ttclient_OnCmdChannelUpdate(Channel chan)
        {
            if ((chan.uChannelType & ChannelType.CHANNEL_CLASSROOM) == ChannelType.CHANNEL_DEFAULT)
                return;
            UpdateUIFromUsers();
        }

        void ttclient_OnCmdUserJoinedChannel(User user)
        {
            UpdateUIFromUsers();
            BroadcastQueueList();
            BroadcastAgenda();
            BroadcastUserStreamTypeList();
        }

        void ttclient_OnCmdUserLeftChannel(User user)
        {   
        
            UpdateUIFromUsers();
        }

        void ttclient_OnCmdUserLoggedIn(User user)
        {
            UserControls.UserLabel item = new UserControls.UserLabel();

            item.UserName = user.szNickname; 
            Channel channel = new Channel();
            ttclient.GetChannel(user.nChannelID, ref channel);

            if (user.uUserState.HasFlag(UserState.USERSTATE_MUTE_VOICE))
            {
                item.Muted = true;
            } 
            else 
            {
                item.Muted = false;
            }


            // Check if user is moderator
            if ((user.uUserType & UserType.USERTYPE_ADMIN) == UserType.USERTYPE_ADMIN) 
            {
                item.Moderator = true;

                userManager = new UserManager(new FileStorage<UserData>(System.IO.Path.GetTempPath() + "tt_user_log"));
                ttclient.OnUserStateChange += new TeamTalk.UserUpdate(handleOnUserStateChange);


            } else 
            { 
                item.Moderator = false;
            }

            if (channel.uChannelType.HasFlag(ChannelType.CHANNEL_CLASSROOM))
            {
                item.Classroom = true;
            }
            else
            {
                item.Classroom = false;
            }

            item.Tag = user.nUserID;

            item.MuteSelectedUser += item_MuteSelectedUser;
            item.AllowSelectedUser += item_AllowSelectedUser;
            item.MessageSelectedUser += item_MessageSelectedUser;
            item.TransmitVideo += item_TransmitVideo;
            try
            {
                userLabels.Add(user.nUserID.ToString(), item);
            }
            catch { }
            UpdateUIFromUsers();
        }

        void item_TransmitVideo(object sender, EventArgs e)
        {
            EventHandler handler = ToggleVideoTransmission;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        void item_MessageSelectedUser(object sender, EventArgs e)
        {                        
            User user = new User();
            if (ttclient.GetUser((int)((UserControls.UserLabel)sender).Tag, ref user))
            {
                int userid = user.nUserID;
                EventHandler handler = MessageUser;
                if (handler != null)
                {
                    handler(this, new TeamTalkApp.NET.Utils.ChatParser.MessageEventArgs(userid.ToString()));
                }
            }
        }

        void item_AllowSelectedUser(object sender, EventArgs e)
        {
            User user = new User();
            Channel chan = new Channel();
            if (ttclient.GetUser((int)((UserControls.UserLabel)sender).Tag, ref user)) {
                if (!userStreamType.ContainsKey(user.nUserID.ToString()))
                    userStreamType.Add(user.nUserID.ToString(), StreamType.STREAMTYPE_VOICE);
                else
                    userStreamType.Remove(user.nUserID.ToString());
            }
            userQueue.Remove(user.nUserID.ToString());
            BroadcastQueueList();
            BroadcastUserStreamTypeList();
            UpdateUIFromUsers();
        }

        void item_MuteSelectedUser(object sender, EventArgs e)
        {
            User user = new User();

            Channel channel = new Channel();
            ttclient.GetChannel(user.nChannelID, ref channel);
            if (ttclient.GetUser((int)((UserControls.UserLabel)sender).Tag, ref user))
                ttclient.SetUserMute(user.nUserID, StreamType.STREAMTYPE_VOICE, !user.uUserState.HasFlag(UserState.USERSTATE_MUTE_VOICE));
            UpdateUIFromUsers();
        }

        void ttclient_OnCmdUserLoggedOut(User user)
        {
            userLabels.Remove(user.nUserID.ToString());
            if (userQueue.ContainsKey(user.nUserID.ToString()))
            {
                userQueue.Remove(user.nUserID.ToString());
            }
        }

        void ttclient_OnCmdUserUpdate(User user)
        {
            UpdateUIFromUsers();
        }

        protected void UpdateUIFromUsers()
        {
            if (ttclient.ChannelID == settings.ModeratorChannel.ID)
            {
                EventHandler handler = InExpertChannel;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            else
            {
                EventHandler handler = InOriginChannel;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
            foreach (UserControls.UserLabel item in userLabels.Values) {
                if (item == null)
                    return;
                User user = new User();
                Boolean gotUser = ttclient.GetUser((int)(item.Tag),ref user);
                item.UserName = user.szNickname;
                Channel channel = new Channel();
                ttclient.GetChannel(user.nChannelID, ref channel);
                //channel.transmitUsers;

                if (channel.uChannelType.HasFlag(ChannelType.CHANNEL_CLASSROOM) && !userStreamType.ContainsKey(user.nUserID.ToString()))
                {
                    item.Muted = true;
                }
                else
                {
                    item.Muted = false;
                }

                if (channel.uChannelType.HasFlag(ChannelType.CHANNEL_CLASSROOM))
                {
                    item.Classroom = true;
                }
                else
                {
                    item.Classroom = false;
                }

                if ((user.uUserType & UserType.USERTYPE_ADMIN) == UserType.USERTYPE_ADMIN)
                {
                    item.Moderator = true;
                    if (ttclient.GetMyChannelID() == user.nChannelID)
                    {
                        currentChannelModerator = user.nUserID;
                    } 
                }
                else
                {
                    item.Moderator = false;
                }

                if (user.nUserID == ttclient.UserID)
                {
                    item.Me = true;
                    item.Speaking = this.VoiceEnabled;
                }
                else
                {
                    item.Me = false;
                }


                if ((ttclient.UserType & UserType.USERTYPE_ADMIN) == UserType.USERTYPE_ADMIN)
                {
                   
                    item.setIsCurrentUserModerator(true);
                }
                else { 
                    item.setIsCurrentUserModerator(false);
                }


                if (userQueue.ContainsKey(user.nUserID.ToString()))
                {
                    item.SetNumberIcon(userQueue[user.nUserID.ToString()]);
                } else {
                    item.SetNumberIcon(-1);
                }

                if (userManager != null)
                {
                    item.SetSpeakingTime(userManager.GetUserTime(user.nUserID));
                }

                
                item.Tag = user.nUserID;

                //if (item != null)
                //    item.Speaking = user.uUserState.HasFlag(UserState.USERSTATE_VOICE | UserState.USERSTATE_MEDIAFILE_AUDIO) ? true : false;


                if (ttclient.GetMyChannelID() != user.nChannelID || ttclient.GetMyChannelID() == 0)
                {
                    if (listview.Controls.Contains(item)) listview.Controls.Remove(item);

                }
                else if (!listview.Controls.Contains(item)) listview.Controls.Add(item);
            }
            
        }


        private void handleOnUserStateChange(User user)
        {
            int userID = user.nUserID;
            bool speaking = user.uUserState.HasFlag(UserState.USERSTATE_VOICE);

            userManager.AddUserState(userID, speaking, user);
            UpdateUIFromUsers();
        }

        private bool voiceEnabled = false;
        public bool VoiceEnabled
        {
            get
            {
                return voiceEnabled;
            }
            set
            {
                voiceEnabled = value;
                UpdateUIFromUsers();
                
            }
        }
    }
}
