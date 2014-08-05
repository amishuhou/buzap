using Newtonsoft.Json;

namespace UsedParts.Domain
{

    public class AccountInfo
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public int OrganizationType { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "offers")]
        public int OffersCount { get; set; }

        [JsonProperty(PropertyName = "cars")]
        public int CarsCount { get; set; }

        [JsonProperty(PropertyName = "vcurr")]
        public double Balance { get; set; }
    }

}
