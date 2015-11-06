using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using TeamTalkLib.NET;
using TeamTalkLib.NET.Decryption;
using TeamTalkLib.Settings;
using System.Net.Http;

namespace TeamTalkLib
{
    public class LinkProvider
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        
        public static ConnectionSettings GetFromActiveLink(string activeLink)
        {
            string activeLinkGUID = RemoveActiveLinkTokens(activeLink);
            string encodedString = GetDataFromURI(activeLinkGUID);
            return Decode(encodedString);
        }

        private static string GetDataFromURI(String URL)
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = client.GetAsync(String.Format("http://{0}", URL)).Result;  // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                log.Error("Encoded String: {0}", response.Content.ReadAsStringAsync().Result);
                return response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                log.Error("Error getting data from {0}", URL);
                return null;
            }  

        }

        public static string GetActiveLink(ConnectionSettings settings)
        {
            string encoded = Encode(settings);
            HttpClient client = new HttpClient();
            StringContent content = new StringContent(encoded, Encoding.UTF8, "text/plain");


            HttpResponseMessage response = client.PostAsync("http://194.29.167.133:80/TT4J/add", content).Result; // Blocking call!
            if (response.IsSuccessStatusCode)
            {
                var uuid = response.Content.ReadAsStringAsync().Result;
                var activeLink = String.Format("wdialogu://http://194.29.167.133:80/TT4J/{0}", uuid);

                return activeLink;
            }
            else
            {
                log.Error("Error with post method");
                return null;
            }

        }

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

        public static ConnectionSettings getTestSettingsForUser(ChannelSettings startup, ChannelSettings moderator, UserSettings user)
        {
            var settings = new ConnectionSettings();

            settings.User = user;
            settings.StartUPChannel = startup;
            settings.ModeratorChannel = moderator;
            settings.Server = new ServerSettings("153.19.141.166", 7077, 7077, 7077, 7077, false);

            return settings;
        }
       
        private static string Encode(ConnectionSettings userSettings)
        {

            RSAHelper helper = new RSAHelper();
            ConnectionSettings copy = userSettings.DeepClone();
            copy.StartUPChannel.Password = helper.EncryptShort(copy.StartUPChannel.Password);
            copy.ModeratorChannel.Password = helper.EncryptShort(copy.ModeratorChannel.Password);
            copy.User.Password = helper.EncryptShort(copy.User.Password);

            string json = JsonConvert.SerializeObject(copy, Formatting.Indented);
            string encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            return encoded;
        }
        private static ConnectionSettings Decode(string encodedString)
        {

            string decodedString = null;
            JObject json = null;
            try
            {
                // string is encoded with base64, so it has to be decoded
                byte[] data = Convert.FromBase64String(encodedString);

                decodedString = Encoding.UTF8.GetString(data);
               

                json= JObject.Parse(decodedString);
                ConnectionSettings settings = JsonConvert.DeserializeObject<ConnectionSettings>(json.ToString());

                var decoder = new RSAHelper();
                settings.StartUPChannel.Password = decoder.DecryptShort(settings.StartUPChannel.Password);
                settings.ModeratorChannel.Password = decoder.DecryptShort(settings.ModeratorChannel.Password);
                settings.User.Password = decoder.DecryptShort(settings.User.Password);

                return settings;

            }
            catch (Exception exc)
            {
                log.Error("Encoded String: {0}", encodedString);
                log.Error("Decoded string: {0}", decodedString);
                log.Error("JSON string: {0}", json);

                throw new ArgumentException("Mallformated activelink: {0}", exc.Message);
            }
        }        

        private static string RemoveActiveLinkTokens(string al)
        {
            int start_index = al.LastIndexOf("://") + 3;
            int end_index = al.Length;
            string tmp = al.Substring(start_index, end_index - start_index);

            return  tmp; ;
        }
    }
}