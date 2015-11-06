using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamTalkApp.Utils;
using TeamTalkLib;
using TeamTalkLib.Settings;

namespace Tests
{
    [TestClass]
    public class LinkProviderTests
    {

        public static ConnectionSettings getMockSettings()
        {
            // User info
            string login = "antek";
            string password = "antek";
            string nick = "A1";
            UserSettings user = new UserSettings(nick, login, password);

            //Server info
            string IP = "153.19.141.166";
            int TCPPort = 7077;
            int UDPPort = 7077;
            int LocalTcpPort = 7077;
            int LocalUdpPort = 7077;
            bool Encrypted = false;
            ServerSettings server = new ServerSettings(IP, TCPPort, UDPPort, LocalTcpPort, LocalUdpPort, Encrypted);

            //Startuo chanel info
            ChannelSettings startUpChannel = new ChannelSettings(1, "");

            //Startuo chanel info
            ChannelSettings modChannel = new ChannelSettings(2, "test123");

            //Connection settings;
            ConnectionSettings cSharpSettings = new ConnectionSettings();
            cSharpSettings.Server = server;
            cSharpSettings.User = user;
            cSharpSettings.StartUPChannel = startUpChannel;
            cSharpSettings.ModeratorChannel = modChannel;

            return cSharpSettings;
        }


        [TestMethod]
        public void Connection_settings_are_consistent_with_Java()
        {

            ConnectionSettings cSharpSettings = getMockSettings();

            // ActiveLink string from java
            string javaLink = "wdialogu://http://localhost:8080/TT4J/2a13c592-5a27-4a00-b275-52ec51ed7854";
            ConnectionSettings javaSettings = LinkProvider.GetFromActiveLink(javaLink);            

            Assert.AreEqual(cSharpSettings, javaSettings);
        }

        [TestMethod]
        public void Can_encode_decode()
        {
        
            var orginal = getMockSettings();            
            var encoded= LinkProvider.GetActiveLink(orginal);
            var decoded = LinkProvider.GetFromActiveLink(encoded);
            
            Assert.AreEqual(orginal, decoded);
        }

        [TestMethod]
        public void TestMockWeb()
        {
            var activelink = LinkProvider.GetActiveLink(LinkProvider.getTestSettingsForUser(
                 new ChannelSettings(1, ""),
                 new ChannelSettings(2, "test123"),
                 new UserSettings("Antek Sybkuwicz", "antek", "antek")));

            var settings = LinkProvider.GetFromActiveLink(activelink);

        }
    }
}
