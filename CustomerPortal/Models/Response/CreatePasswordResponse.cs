using Newtonsoft.Json;

namespace CustomerPortal.Models.Response
{
    public class CreatePasswordResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("dataCount")]
        public int DataCount { get; set; }

    }
}
