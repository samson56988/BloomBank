using Newtonsoft.Json;

namespace CustomerAPI.Models.Response
{
    public class BankTransferResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("dataCount")]
        public int DataCount { get; set; }

        [JsonProperty("data")]
        public TransferResponse Data { get; set; }
    }

    public class TransferResponse
    {
        public string TransactionReference { set; get; }
        public string SenderName { get; set; }
        public string SenderAccountNumber { get; set; }
        public string PayeeAccountNumber { get; set; }
        public string PayeeName { get; set; }
        public string Narration { get; set; }
        public string PlatForm { get; set; }
        public decimal Amount { get; set; }
    }
}
