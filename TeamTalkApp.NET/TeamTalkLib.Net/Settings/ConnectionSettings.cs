using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamTalkLib.Settings
{
    [Serializable]
    public class ConnectionSettings
    {        
        public ServerSettings Server { get; set; }
        public ChannelSettings StartUPChannel { get; set; }
        public ChannelSettings ModeratorChannel { get; set; }
        public UserSettings User { get; set; }


        #region override Equals
        public override bool Equals(Object obj)
        {
            ConnectionSettings other = obj as ConnectionSettings;
            if (other == null)
                return false;

            if (!Server.Equals(other.Server))
                return false;

            if (!StartUPChannel.Equals(other.StartUPChannel))
                return false;

            if (!ModeratorChannel.Equals(other.ModeratorChannel))
                return false;

            if (!User.Equals(other.User))
                return false;
            
            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;
                hash = hash * 16777619 ^ (Server != null ? Server.GetHashCode() : 0);
                hash = hash * 16777619 ^ (StartUPChannel != null ? StartUPChannel.GetHashCode(): 0);
                hash = hash * 16777619 ^ (User != null ? User.GetHashCode(): 0);
                return hash;
            }
        }
        #endregion

    }
}
