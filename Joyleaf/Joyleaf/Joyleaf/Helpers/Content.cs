using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Joyleaf.Helpers
{
    public partial class Content
    {
        [JsonProperty("curated")]
        public Curated[] Curated { get; set; }

        [JsonProperty("featured")]
        public Item[] Featured { get; set; }
    }

    public partial class Curated
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("items")]
        public Item[] Items { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("info")]
        public Info Info { get; set; }

        [JsonProperty("reviews")]
        public Reviews Reviews { get; set; }
    }

    public partial class Info
    {
        [JsonProperty("desc", NullValueHandling = NullValueHandling.Ignore)]
        public string Desc { get; set; }

        [JsonProperty("effects")]
        public Effects Effects { get; set; }

        [JsonProperty("flavors")]
        public Dictionary<string, string> Flavors { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("race")]
        public Race Race { get; set; }
    }

    public partial class Effects
    {
        [JsonProperty("medical", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Medical { get; set; }

        [JsonProperty("negative", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Negative { get; set; }

        [JsonProperty("positive", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, string> Positive { get; set; }
    }

    public enum Race { Hybrid, Indica, Sativa };

    public partial class Content
    {
        public static Content FromJson(string json) => JsonConvert.DeserializeObject<Content>(json, Content_Converter.Settings);
    }

    public static class Content_Serialize
    {
        public static string ToJson(this Content self) => JsonConvert.SerializeObject(self, Content_Converter.Settings);
    }

    internal static class Content_Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                Content_RaceConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class Content_RaceConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(Race) || t == typeof(Race?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "hybrid":
                    return Race.Hybrid;
                case "indica":
                    return Race.Indica;
                case "sativa":
                    return Race.Sativa;
            }
            throw new Exception("Cannot unmarshal type Race");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (Race)untypedValue;
            switch (value)
            {
                case Race.Hybrid:
                    serializer.Serialize(writer, "hybrid");
                    return;
                case Race.Indica:
                    serializer.Serialize(writer, "indica");
                    return;
                case Race.Sativa:
                    serializer.Serialize(writer, "sativa");
                    return;
            }
            throw new Exception("Cannot marshal type Race");
        }

        public static readonly Content_RaceConverter Singleton = new Content_RaceConverter();
    }
}