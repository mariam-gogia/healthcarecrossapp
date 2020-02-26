namespace UniversalCarePlan
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    public class GuidelinesData
    {

        [JsonProperty("results")]
        public IList<Result> results { get; set; }
    }

    public class Result
    {

        [JsonProperty("date")]
        public string date { get; set; }

        [JsonProperty("code")]
        public string code { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("URL")]
        public string URL { get; set; }

        [JsonProperty("section")]
        public string section { get; set; }

        [JsonProperty("priority")]
        public string priority { get; set; }

        [JsonProperty("recommendation")]
        public string recommendation { get; set; }

    }


}
