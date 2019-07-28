using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Joyleaf.Helpers
{
    public partial class SearchResult
    {
        [JsonProperty("result")]
        public Item[] Items { get; set; }
    }

    public partial class SearchResult
    {
        public static SearchResult FromJson(string json) => JsonConvert.DeserializeObject<SearchResult>(json, SearchResult_Converter.Settings);
    }

    public static class SearchResult_Serialize
    {
        public static string ToJson(this SearchResult self) => JsonConvert.SerializeObject(self, SearchResult_Converter.Settings);
    }

    internal static class SearchResult_Converter
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
