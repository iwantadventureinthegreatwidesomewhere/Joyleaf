using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Joyleaf.Helpers
{
    public class Log
    {
        public Dictionary<string, int> Topics = new Dictionary<string, int>();
        public Dictionary<string, int> Tags = new Dictionary<string, int>();

        public static void AddTopic(string str)
        {
            Log log;

            if (!Settings.Log.Equals(""))
            {
                log = JsonConvert.DeserializeObject<Log>(Settings.Log);
            }
            else
            {
                log = new Log();
            }

            if (str.Equals("Social"))
            {
                str = "Talkative";
            }else if (str.Equals("Spicy/Herbal"))
            {
                str = "Spicy";
            }

            if (log.Topics.ContainsKey(str))
            {
                log.Topics[str] = log.Topics[str] + 1;
            }
            else
            {
                log.Topics.Add(str, 1);
            }

            Settings.Log = JsonConvert.SerializeObject(log);
        }

        public static void AddTag(string str)
        {
            Log log;

            if (!Settings.Log.Equals(""))
            {
                log = JsonConvert.DeserializeObject<Log>(Settings.Log);
            }
            else
            {
                log = new Log();
            }

            if (log.Tags.ContainsKey(str))
            {
                log.Tags[str] = log.Tags[str] + 1;
            }
            else
            {
                log.Tags.Add(str, 1);
            }

            Settings.Log = JsonConvert.SerializeObject(log);
        }

        public static Log GetLog()
        {
            if (!Settings.Log.Equals(""))
            {
                return JsonConvert.DeserializeObject<Log>(Settings.Log);
            }

            return new Log();
        }

        public static void Reset()
        {
            Settings.Log = "";
        }
    }
}
