using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Globalization;

namespace Joyleaf.Helpers
{
    public partial class Content
    {
        [JsonProperty("data")]
        public Datum[] Data { get; set; }
    }

    public class Datum
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }
    }

    public class Item
    {
        [JsonProperty("availability")]
        public bool Availability { get; set; }

        [JsonProperty("brand")]
        public string Brand { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("cbd")]
        public string Cbd { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("species")]
        public string Species { get; set; }

        [JsonProperty("thc")]
        public string Thc { get; set; }

        [JsonProperty("url")]
        public Uri Url { get; set; }
    }

    public partial class Content
    {
        public static Content FromJson(string json) => JsonConvert.DeserializeObject<Content>(json, Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Content self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

    internal static class Converter
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