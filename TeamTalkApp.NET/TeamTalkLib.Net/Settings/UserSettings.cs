using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamTalkLib.Settings
{
    [Serializable]
    public class UserSettings
    {
        public string Nick { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public UserSettings(string nick, string login, string password)
        {
            Nick = nick;
            Login = login;
            Password = password;
        }

        #region override Equals
        public override bool Equals(Object obj)
        {
            UserSettings other = obj as UserSettings;
            if (other == null)
                return false;

            if (!Login.Equals(other.Login))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = hash * 16777619 ^ Nick.GetHashCode();
                hash = hash * 16777619 ^ Login.GetHashCode();
                hash = hash * 16777619 ^ Password.GetHashCode();
                return hash;
            }
        }
        #endregion
    }
}

