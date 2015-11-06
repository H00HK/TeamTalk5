using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamTalkLib.Settings
{
    [Serializable]
    public class ChannelSettings
    {
        public int ID { get; set; }
        public string Password { get; set; }

        public ChannelSettings(int id, string password)
        {
            ID = id;
            Password = password;
        }

        #region override Equals
        public override bool Equals(Object obj)
        {
            ChannelSettings other = obj as ChannelSettings;
            if (other == null)
                return false;

            if (!ID.Equals(other.ID))
                return false;

            if (!Password.Equals(other.Password))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = (int)2166136261;                
                hash = hash * 16777619 ^ ID.GetHashCode();
                hash = hash * 16777619 ^ Password.GetHashCode();                
                return hash;
            }
        }
        #endregion
    }
}
