using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Joyleaf.Helpers
{
    public partial class Reviews
    {
        [JsonProperty("averageRating")]
        public double AverageRating { get; set; }

        [JsonProperty("numberOfReviews")]
        public long NumberOfReviews { get; set; }

        [JsonProperty("ratings")]
        public Dictionary<string, Rating> Ratings { get; set; }
    }

    public partial class Rating
    {
        [JsonProperty("poster_name")]
        public string PosterName { get; set; }

        [JsonProperty("score")]
        public double Score { get; set; }

        [JsonProperty("review")]
        public string Review { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }
    }

    public partial class Reviews
    {
        public static Reviews FromJson(string json) => JsonConvert.DeserializeObject<Reviews>(json, Reviews_Converter.Settings);
    }

    public static class Reviews_Serialize
    {
        public static string ToJson(this Reviews self) => JsonConvert.SerializeObject(self, Reviews_Converter.Settings);
    }

    internal static class Reviews_Converter
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