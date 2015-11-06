using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using TeamTalkApp.Utils;
using TeamTalkLib;
using TeamTalkLib.Settings;

namespace Tests
{
    [TestClass]
    public class ArgumentParserTests
    {

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Active link cannot be null")]
        public void Should_have_non_null_arguments()
        {
            ArgumentParser parser = new ArgumentParser(null);
        }

        [TestMethod]
        public void Should_Parse_ActiveLink()
        {
            var orginal = LinkProviderTests.getMockSettings();
            var activeLink = LinkProvider.GetActiveLink(orginal);
            var parser = new ArgumentParser(new string[] { "test", activeLink });
            var outcome = parser.CommonSettings;

            Assert.AreEqual(orginal, outcome);

        }

        [TestMethod]
        public void Can_Handle_Backslash() { 
            try {

            string activeLink = "wdialogu://http://localhost:8080/TT4J/b3533d71-b749-458a-8b1a-5d6d1a44bea6/";
            var parser = new ArgumentParser(new string[] { "test", activeLink });
            var outcome = parser.CommonSettings;
            } catch(Exception exc){
                Assert.Fail("Expected no exception, but got: " + exc.Message);
            }
        }

        [TestMethod]
        public void Test_Java_Encoding()
        {
        
            string activeLink = "wdialogu://http://localhost:8080/TT4J/b3533d71-b749-458a-8b1a-5d6d1a44bea6";


            var parser = new ArgumentParser(new string[] { "test", activeLink });
            var outcome = parser.CommonSettings;

            Assert.AreNotEqual(null, outcome);

        }


    }
}
