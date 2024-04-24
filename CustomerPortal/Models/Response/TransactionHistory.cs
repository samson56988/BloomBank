using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CustomerPortal.Models.Response
{
    public class TransactionHistory
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("dataCount")]
        public int DataCount { get; set; }

        [JsonProperty("data")]
        public List<TransactionsRecord> Data { get; set; }
    }

    public class TransactionsRecord
    {
        [JsonProperty("accountNo")]
        public string AccountNo { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("transactionRef")]
        public string TransactionRef { get; set; }

        [JsonProperty("beneficiaryAccountNo")]
        public string BeneficiaryAccountNo { get; set; }

        [JsonProperty("beneficiaryAccountName")]
        public string BeneficiaryAccountName { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("narration")]
        public string Narration { get; set; }

        [JsonProperty("dateCreated")]
        public DateTime DateCreated { get; set; }

        [JsonProperty("apiResponse")]
        public object ApiResponse { get; set; }

        [JsonProperty("transactionStatus")]
        public string TransactionStatus { get; set; }

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("apiUrl")]
        public string ApiUrl { get; set; }

        [JsonProperty("isSuccessful")]
        public bool IsSuccessful { get; set; }

        [JsonProperty("isReversed")]
        public bool IsReversed { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        public string TransactionType { get; set; }
    }

}
