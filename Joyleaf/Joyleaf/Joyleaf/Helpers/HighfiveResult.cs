using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Joyleaf.Helpers
{
    public partial class HighfiveResult
    {
        [JsonProperty("result")]
        public Result[] Result { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("item")]
        public Item Item { get; set; }

        [JsonProperty("matchPercent")]
        public double MatchPercent { get; set; }
    }

    public partial class HighfiveResult
    {
        public static HighfiveResult FromJson(string json) => JsonConvert.DeserializeObject<HighfiveResult>(json, HighfiveResult_Converter.Settings);
    }

    public static class HighfiveResult_Serialize
    {
        public static string ToJson(this HighfiveResult self) => JsonConvert.SerializeObject(self, HighfiveResult_Converter.Settings);
    }

    internal static class HighfiveResult_Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
