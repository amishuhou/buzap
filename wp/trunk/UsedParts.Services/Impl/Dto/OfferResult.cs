using System.Collections.Generic;
using Newtonsoft.Json;
using UsedParts.Domain;

namespace UsedParts.Services.Impl.Dto
{
    public class OffersResult
    {
        [JsonProperty(PropertyName = "items")]
        public IEnumerable<Offer> Items { get; set; }
    }
}
