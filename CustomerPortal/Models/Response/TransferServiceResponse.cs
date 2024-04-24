using Newtonsoft.Json;

namespace CustomerPortal.Models.Response
{
    public class TransferServiceResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("dataCount")]
        public int DataCount { get; set; }

        [JsonProperty("data")]
        public TransferDetails Data { get; set; }
    }

    public class TransferDetails
    {

    }
}
