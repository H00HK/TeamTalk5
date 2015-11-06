using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamTalkLib.NET.Decryption;

namespace Tests
{
    [TestClass]
    public class RSAHelperTests
    {
        [TestMethod]
        public void Decoded_string_is_consistent_with_java()
        {
            RSAHelper dec = new RSAHelper();
            string data2Decrypt = "CBNrBIpdt/0KYcMgn4iFoQrJj7sU2GGdoXJ/sLIB7JMrN5sO7cqP6XbjCxHzl9f9FE9VUcq4LiZOstqndcnS866tN69It/8vxjdyZ0tEmEiuphRGmYEgjqYV68SMl2q4Jo5sWSjudh7SV3/P1G2IxDxEcYv6MCceLBJK1Y3BDyI=";
            string result = dec.DecryptShort(data2Decrypt);

            Assert.AreEqual("Hello!", result);
        }

        [TestMethod]
        public void Is_able_to_encode_decode()
        {
            RSAHelper dec = new RSAHelper();
            string raw = "Hello, it's a me: Mario!";
            string encrypted = dec.EncryptShort(raw);
            string decrypted = dec.DecryptShort(encrypted);

            Assert.AreEqual(raw, decrypted);
        }
    }
}
