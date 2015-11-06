using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TeamTalkLib.NET.Decryption
{
    public class RSAHelper
    {
        const int PROVIDER_RSA_FULL = 1;
        const string CONTAINER_NAME = "Tracker";
        private RSACryptoServiceProvider rsa;

        public RSAHelper()
        {

            CspParameters cspParams = new CspParameters(PROVIDER_RSA_FULL);
            cspParams.KeyContainerName = CONTAINER_NAME;
            rsa = new RSACryptoServiceProvider(cspParams);
            rsa.FromXmlString(Properties.Settings.Default.PrivateKey);                                                                                
        }

        public string DecryptShort(string data2Decrypt)
        {
            byte[] encyrptedBytes = Convert.FromBase64String(data2Decrypt);
            byte[] plain = rsa.Decrypt(encyrptedBytes, false);
            string decryptedString = Encoding.UTF8.GetString(plain);
            return decryptedString;
        }

        public string EncryptShort(string data2Encrypt)
        {
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            byte[] textBytes = encoding.GetBytes(data2Encrypt);
            byte[] encryptedOutput = rsa.Encrypt(textBytes, false);
            string outputB64 = Convert.ToBase64String(encryptedOutput);
            return outputB64;
        }


    }
}
