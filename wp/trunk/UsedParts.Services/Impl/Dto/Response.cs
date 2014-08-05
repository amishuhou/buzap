using Newtonsoft.Json;

namespace UsedParts.Services.Impl.Dto
{
    public class Response<T> where T : class
    {
        [JsonProperty(PropertyName = "info")]
        public T Data { get; set; }

        [JsonProperty(PropertyName = "err")]
        public int ErrorCode { get; set; }

        [JsonProperty(PropertyName = "errstr")]
        public string ErrorDescription { get; set; }
    }
}
