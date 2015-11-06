using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TeamTalkLib.Settings
{
    [Serializable]
    public class ServerSettings
    {
        public string IP { get; set; }
        public int TCPPort { get; set; }
        public int UDPPort { get; set; }
        public int LocalTcpPort { get; set; }
        public int LocalUdpPort { get; set; }
        public bool Encrypted { get; set; }

        public ServerSettings(string ip, int tcpPort, int udpPort, int localTcpPort, int localUdpPort, bool encrypted)
        {
            IP = ip;
            TCPPort = tcpPort;
            UDPPort = udpPort;
            LocalTcpPort = localTcpPort;
            LocalUdpPort = localUdpPort;
            Encrypted = encrypted;

        }

        #region override Equals
        public override bool Equals(Object obj)
        {
            ServerSettings other = obj as ServerSettings;
            if (other == null)
                return false;

            if (!IP.Equals(other.IP))
                return false;

            if (!TCPPort.Equals(other.TCPPort))
                return false;

            if (!UDPPort.Equals(other.UDPPort))
                return false;

            if (!LocalTcpPort.Equals(other.LocalTcpPort))
                return false;

            if (!LocalUdpPort.Equals(other.LocalUdpPort))
                return false;

            if (!Encrypted.Equals(other.Encrypted))
                return false;

            return true;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = (int)2166136261;                
                hash = hash * 16777619 ^ IP.GetHashCode();
                hash = hash * 16777619 ^ TCPPort.GetHashCode();
                hash = hash * 16777619 ^ LocalTcpPort.GetHashCode();
                hash = hash * 16777619 ^ LocalUdpPort.GetHashCode();
                hash = hash * 16777619 ^ Encrypted.GetHashCode();
                return hash;
            }
        }
        #endregion

    }
}
