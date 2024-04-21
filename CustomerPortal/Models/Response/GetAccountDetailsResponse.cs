using Newtonsoft.Json;

namespace CustomerPortal.Models.Response
{ 
    public class GetAccountDetailsResponse
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("dataCount")]
        public int DataCount { get; set; }

        [JsonProperty("data")]
        public AccountDetails Data { get; set; }
    }

    public class AccountDetails
    {
        [JsonProperty("accountNo")]
        public string AccountNo { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("middleName")]
        public string MiddleName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("bvn")]
        public string BVN { get; set; }

        [JsonProperty("accountType")]
        public string AccountType { get; set; }

        [JsonProperty("identificationType")]
        public string IdentificationType { get; set; }

        [JsonProperty("identificationNumber")]
        public string IdentificationNumber { get; set; }

        [JsonProperty("transactionLimit")]
        public decimal TransactionLimit { get; set; }

        [JsonProperty("dateOnboarded")]
        public DateTime DateOnboarded { get; set; }

        [JsonProperty("accountBalance")]
        public decimal AccountBalance { get; set; }

        [JsonProperty("cifNo")]
        public string CIFNo { get; set; }

        [JsonProperty("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty("hasPND")]
        public bool HasPND { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }
    }
}
