using System.Collections.Generic;
using Newtonsoft.Json;

namespace UsedParts.Domain
{
    public class OrdersResult
    {
        [JsonProperty(PropertyName = "pages")]
        public int Pages { get; set; }

        [JsonProperty(PropertyName = "total")]
        public int Total { get; set; }

        [JsonProperty(PropertyName = "items")]
        public IEnumerable<Order> Items { get; set; }
    }
}
