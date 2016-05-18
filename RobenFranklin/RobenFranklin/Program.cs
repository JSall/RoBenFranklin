using System;
using System.Collections.Generic;
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
            MarkovChain chain = new MarkovChain(10);
            FeedText(chain);
            List<string> tweets = new List<string>();

            var twitterApp = new TwitterService("K47rQlImnCUoRFTnVtB2yJfaN", "5Wv4cjWfRsWZO5LuWLxldCh0AU8GBYQVAyfODnhzhkGEp5r1BG");
            twitterApp.AuthenticateWith("722475012255563776-Ukvwm59B9U94AGo014K0TJA2h5zUWot", "v1qWB8Bs9KPZHyQccHozpsGb85wHrRGYW76lJEsdCd6Y9");

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
