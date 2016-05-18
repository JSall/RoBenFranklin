using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
            AppSettingsReader settingsReader =new AppSettingsReader();

            MarkovChain chain = new MarkovChain(10);
            FeedText(chain);
            List<string> tweets = new List<string>();

            var twitterApp = new TwitterService((string)settingsReader.GetValue("ConsumerKey",
                                                     typeof(String)),
                                                (string)settingsReader.GetValue("ConsumerSecret",
                                                     typeof(String)));

            twitterApp.AuthenticateWith((string)settingsReader.GetValue("TokenKey",
                                                     typeof(String)),
                                                (string)settingsReader.GetValue("TokenSecret",
                                                     typeof(String)));

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
    }
}
