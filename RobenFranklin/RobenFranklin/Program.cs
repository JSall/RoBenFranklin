using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TweetSharp;

namespace RobenFranklin
{
    class Program
    {
        static void Main(string[] args)
        {

            UpdateRoBenFranklin();


        }

        private static void UpdateRoBenFranklin()
        {
            AppSettingsReader settingsReader = new AppSettingsReader();
            Encryption enc = new Encryption();

            MarkovChain chain = new MarkovChain(10);
            FeedText(chain);
            List<string> tweets = new List<string>();

            var twitterApp = new TwitterService(enc.Decrypt((string)settingsReader.GetValue("ConsumerKey",
                                                     typeof(String))),
                                                enc.Decrypt((string)settingsReader.GetValue("ConsumerSecret",
                                                     typeof(String))));

            twitterApp.AuthenticateWith(enc.Decrypt((string)settingsReader.GetValue("TokenKey",
                                                     typeof(String))),
                                               enc.Decrypt((string)settingsReader.GetValue("TokenSecret",
                                                     typeof(String))));

            for (int i = 1; i <= 15; i++)
            {
                string tweet = chain.ToString(5);
                tweets.Add(tweet);
                twitterApp.SendTweet(new SendTweetOptions
                {
                    Status = tweet
                });

            }

        }

        private static void FeedText(MarkovChain model)
        {
            StringBuilder Feeder = new StringBuilder();
            string line;
            using (StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "TextFiles\\BenFranklin.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {

                    var l = line.Trim();
                    if (l.Length > 3)
                        Feeder.AppendLine(l);
                    else if (Feeder.Length > 0)
                    {
                        model.AddString(Feeder.ToString());
                        Feeder.Length = 0;

                    }

                }
                if (Feeder.Length > 0)
                    model.AddString(Feeder.ToString());
            }

        }

        //    public static void GenerateRsa(string privateKeyPath, string publicKeyPath, int size)
        //    {
        //        //stream to save the keys
        //        FileStream fs = null;
        //        StreamWriter sw = null;

        //        //create RSA provider
        //        RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(size);
        //        try
        //        {
        //            //save private key
        //            fs = new FileStream(privateKeyPath, FileMode.Create, FileAccess.Write);
        //            sw = new StreamWriter(fs);
        //            sw.Write(rsa.ToXmlString(true));
        //            sw.Flush();
        //        }
        //        finally
        //        {
        //            if (sw != null) sw.Close();
        //            if (fs != null) fs.Close();
        //        }

        //        try
        //        {
        //            //save public key
        //            fs = new FileStream(publicKeyPath, FileMode.Create, FileAccess.Write);
        //            sw = new StreamWriter(fs);
        //            sw.Write(rsa.ToXmlString(false));
        //            sw.Flush();
        //        }
        //        finally
        //        {
        //            if (sw != null) sw.Close();
        //            if (fs != null) fs.Close();
        //        }
        //        rsa.Clear();
        //    }
    }
}
