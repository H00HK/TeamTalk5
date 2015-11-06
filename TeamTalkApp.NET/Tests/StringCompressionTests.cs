using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamTalkApp.NET.Utils;

namespace Tests
{
    [TestClass]
    public class StringCompressionTests
    {
        [TestMethod]
        public void Can_encode_decode()
        {
            string orginal = "This is a string";

            string compressed = StringCompression.Compress(orginal);
            string decompressed = StringCompression.Decompress(compressed);

            Assert.AreEqual(orginal, decompressed);

        }

        [TestMethod]
        public void Is_consistent_with_java()
        {
            string orginal = "{\"User\":{\"Nick\":\"user\",\"Login\":\"user\",\"Password\":\"password\"},\"Channel\":{\"ID\":2,\"Password\":\"test123\"},\"Server\":{\"IP\":\"153.19.141.166\",\"TCPPort\":7077,\"UDPPort\":7077,\"LocalTcpPort\":7077,\"LocalUdpPort\":7077,\"Encrypted\":false}}";
            string cSharpCompression = StringCompression.Compress(orginal);
            string java = "3gAAAB+LCAAAAAAAAABljdEKwiAUht/lXIvkVpO83boYjBCaDyDOajRU1IoYe/ccESHdne87/zn/DCJoD2yG46huwOC+IoLOXkbzQy5DeFo/JOO+44Kgvkpj9LSetw2wIgtGHSIpyjV30v7xaWl52pBdickeky3BpKrS+77m3PoIjG4oRSCaDDur5NQr9+fEkLmDUf7lok7lZzkFvSxvJyNsYt4AAAA=";
            string decompressed = StringCompression.Decompress(java);

            Assert.AreEqual(orginal, decompressed);
        }
    }
}
