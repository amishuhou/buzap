using Newtonsoft.Json;

namespace UsedParts.Services.Impl.Dto
{
    public class AuthResult
    {
        [JsonProperty(PropertyName = "loged")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
