using Newtonsoft.Json;

namespace UsedParts.Domain
{
    public class Manufacturer
    {
        [JsonProperty(PropertyName = "mfa_id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "mfa_txt")]
        public string Name { get; set; }
    }
}
