using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamTalkLib.NET.Recording
{
    public class UserData
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public TimeSpan SpeakingTime { get; set; }
        public UserData(int id, TimeSpan speakingTime, string username, string nickname)
        {
            ID = id;
            SpeakingTime = speakingTime;
            Username = username;
            Nickname = nickname;

        }

        override public string ToString()
        {
            return String.Format("User {0} with id: {1} and nickname {2}, speaking time: {3}",  Username, ID, Nickname, SpeakingTime);
        }
    }
}
