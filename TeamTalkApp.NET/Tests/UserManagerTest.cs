using System.Threading;
using BearWare;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamTalkLib;
using TeamTalkLib.NET.Recording;
using TeamTalkLib.Storage;

namespace Tests
{
    [TestClass]
    public class UserManagerTest
    {
        [TestMethod]
        public void Should_calcualte_times_proprely()
        {
            UserManager um = new UserManager(new FileStorage<UserData>("user_log"));

            User user1 = new User() { szUsername = "first", szNickname = "foo"};
            User user2 = new User() { szUsername = "second", szNickname = "bar" };

            um.AddUserState(1, true, user1);            
            Thread.Sleep(1500);

            um.AddUserState(2, true, user2);
            Thread.Sleep(100);

            um.AddUserState(1, false, user1);
            Thread.Sleep(100);

            um.AddUserState(2, false, user2);

            string log = um.GetJSONStatistics();

            um.StoreUserStatistics();     
        }
    }
}
