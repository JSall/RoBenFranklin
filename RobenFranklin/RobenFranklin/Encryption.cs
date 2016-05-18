using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RobenFranklin
{
    public class Encryption
    {

        private RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(2048);

        //Create a new instance of the RSAParameters structure.

        public Encryption()
        {
            string privateKey, publicKey;

            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TextFiles\\Privatekey.xml"))
            {
                privateKey = reader.ReadToEnd();
            }
            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TextFiles\\Publickey.xml"))
            {
                publicKey = reader.ReadToEnd();
            }

            RSA.FromXmlString(publicKey);

            RSA.FromXmlString(privateKey);

        }

        public string Encrypt(string plainText)
        {


            byte[] encryptedBytes = RSA.Encrypt(UTF8Encoding.UTF8.GetBytes(plainText), false);

            return Convert.ToBase64String(encryptedBytes);

        }


        public string Decrypt(string text)
        {



            byte[] textAsBytes = Convert.FromBase64String(text);

            byte[] decryptedBytes = RSA.Decrypt(textAsBytes, false);

            return UTF8Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
