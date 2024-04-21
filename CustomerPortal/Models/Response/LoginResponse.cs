using Newtonsoft.Json;

namespace CustomerPortal.Models.Response
{
    public class LoginResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("dataCount")]
        public int DataCount { get; set; }

        [JsonProperty("data")]
        public CustomerDetails Data { get; set; }
    }
}
