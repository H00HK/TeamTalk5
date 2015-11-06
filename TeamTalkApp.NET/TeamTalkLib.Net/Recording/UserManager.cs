using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using BearWare;
using Newtonsoft.Json;
using TeamTalkLib.NET.Recording;

namespace TeamTalkLib
{
    public class UserManager
    {

        private Dictionary<int, Tuple<Stopwatch, User>> users;
        private IStorage<List<UserData>> storage; 

        public UserManager(IStorage<List<UserData>> storage)
        {
            this.users = new Dictionary<int, Tuple<Stopwatch, User>>();
            this.storage = storage;
        }

        public void AddUserState(int userId, bool isSpeaking, User user)
        {
            if (!users.ContainsKey(userId))
            {
                var watch = new Stopwatch();
                if(isSpeaking)
                    watch.Start();

                users.Add(userId, new Tuple<Stopwatch, User>(watch, user));
            } else
            {
                var watch = users[userId].Item1;
                if(watch.IsRunning && !isSpeaking)
                {
                    watch.Stop();
                }

                if(!watch.IsRunning && isSpeaking)
                {
                    watch.Start();
                }
            }
        }

        public TimeSpan GetUserTime(int userId)
        {
            return users.ContainsKey(userId) ? users[userId].Item1.Elapsed : new TimeSpan();
        }

        // Store statistics
        public void StoreUserStatistics()
        {
            storage.Store(GetUserData());
        }


        // User statistics are returned as JSON string
        public string GetJSONStatistics()
        {
            List<UserData> items = GetUserData();
            return JsonConvert.SerializeObject(items, Formatting.Indented);
        }

        private List<UserData> GetUserData()
        {
            var items = new List<UserData>();
            foreach (var item in users.ToList())
            {
                var watch = item.Value.Item1;
                var user = item.Value.Item2;
                items.Add(new UserData(item.Key, watch.Elapsed, user.szUsername, user.szNickname ));
            }

            return items;
        }
    }
}
