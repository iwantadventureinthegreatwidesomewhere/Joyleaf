using System;
using System.Collections.Generic;

namespace Joyleaf.Helpers
{
    public static class Log
    {
        public static Dictionary<string, int> Topics = new Dictionary<string, int>();
        public static Dictionary<string, int> Tags = new Dictionary<string, int>();

        public static void AddTopic(string str)
        {
            if (Topics.ContainsKey(str))
            {
                Topics[str] = Topics[str] + 1;
            }
            else
            {
                Topics.Add(str, 1);
            }
        }

        public static void AddTag(string str)
        {
            if (Tags.ContainsKey(str))
            {
                Tags[str] = Tags[str] + 1;
            }
            else
            {
                Tags.Add(str, 1);
            }
        }

        public static void Reset()
        {
            Topics.Clear();
            Tags.Clear();
        }
    }
}
