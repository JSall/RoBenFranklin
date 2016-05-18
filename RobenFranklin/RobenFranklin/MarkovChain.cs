using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobenFranklin
{
    public class MarkovChain
    {

        internal class MarkovNode
        {



            public char Coursor;
            public int Count;
            public int FollowCount;
            public Dictionary<char, MarkovNode> Children;



            public MarkovNode(char c)
            {
                Coursor = c;
                Count = 1;
                FollowCount = 0;
                Children = new Dictionary<char, MarkovNode>();
            }

            public MarkovNode AddNode(char c)
            {
                ++FollowCount;
                MarkovNode child;
                if (Children.TryGetValue(c, out child))
                    ++child.Count;
                else
                {
                    child = new MarkovNode(c);
                    Children.Add(c, child);
                }
                return child;
            }


        }

        private Random StatePicker = new Random();
        private MarkovNode Root;
        private int ModelOrder;

        public const char StartChar = (char)0xFFFE;
        public const char StopChar = (char)0xFFFF;

        public MarkovChain(int order = 0)
        {
            ModelOrder = order;
            Root = new MarkovNode(StartChar);

            //foreach (char c in text)
            //{
            //    ++Root.Count;
            //    Root.AddNode(c);
            //}

            //Root.AddNode(StopChar);
        }

        public void AddString(string s)
        {
            StringBuilder builder = new StringBuilder(s.Length + 2 * ModelOrder);
            builder.Append(StartChar, ModelOrder);
            builder.Append(s);
            builder.Append(StopChar, ModelOrder);

            for (int iStart = 0; iStart < builder.Length; ++iStart)
            {
                MarkovNode parent = Root.AddNode(builder[iStart]);

                for (int i = 1; i <= ModelOrder && i + iStart < builder.Length; i++)
                {
                    MarkovNode child = parent.AddNode(builder[iStart + i]);
                    parent = child;
                }
            }

        }

        public string ToString(int order)
        {
            StringBuilder result = new StringBuilder();
            result.Append(StartChar, order);
            int iStart = 0;
            char ch = StartChar;
            do
            {
                MarkovNode node = Root.Children[result[iStart]];
                for (int i = 1; i < order; i++)
                {
                    node = node.Children[result[i + iStart]];
                }
                ch = ChildGet(node);
                if (ch != StopChar)
                    result.Append(ch);
                ++iStart;
            }
            while (ch != StopChar);

            string clean = result.ToString().Substring(order);
            if (clean.Contains(StartChar))
                clean = clean.Replace(StartChar.ToString(), "");

            while (clean.Length < 140)
                clean = clean + " " + ToString(order);

            return clean.Substring(0, 140);

        }

        private char ChildGet(MarkovNode node)
        {
            int roll = StatePicker.Next(node.FollowCount);
            int frequency = 0;
            foreach (var kvp in node.Children)
            {
                frequency += kvp.Value.Count;
                if (frequency > roll)
                {
                    return kvp.Key;
                }
            }

            throw new Exception("I shit the bed!");
        }

    }

}

