using Newtonsoft.Json;

namespace UsedParts.Services.Impl.Dto
{
    public class OfferResult
    {
        [JsonProperty(PropertyName = "offId")]
        public int OfferId { get; set; }
    }
}
