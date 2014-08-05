using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UsedParts.Domain.Converters;

namespace UsedParts.Domain
{

    public class Offer
    {
        [JsonProperty(PropertyName = "ix")]
        public long Id { get; set; }

        [JsonProperty(PropertyName = "cond")]
        public int Condition { get; set; }

        [JsonProperty(PropertyName = "deli")]
        public int Delivery { get; set; }

        [JsonProperty(PropertyName = "gara")]
        public int Warranty { get; set; }

        [JsonProperty(PropertyName = "ava")]
        public int Available { get; set; }

        [JsonProperty(PropertyName = "price")]
        public double Price { get; set; }

        [JsonProperty(PropertyName = "offDate")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime Posted { get; set; }

        [JsonProperty(PropertyName = "sellerHead")]
        public string SelllerName { get; set; }

        [JsonProperty(PropertyName = "sellerLogo")]
        public string SelllerLogo { get; set; }

        [JsonProperty(PropertyName = "usrId")]
        public int SellerId { get; set; }

        [JsonProperty(PropertyName = "reqId")]
        public int OrderId { get; set; }

        [JsonProperty(PropertyName = "imgs")]
        public IEnumerable<string> Images { get; set; }

    }

}
