using Newtonsoft.Json;

namespace CustomerAPI.Models.Response
{
    public class CreateCustomerResponse
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

    public class CustomerDetails
    {
        public string AccountNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string BVN { get; set; }
        public string AccountType { get; set; } 
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public decimal TransactionLimit { get; set; }
        public DateTime DateOnboarded { get; set; }
        public decimal AccountBalance { get; set; }
        public string CifNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool HasPND { get; set; }
        public string CurrencyCode { get; set; }
        public string AccountName { get; set; }
    }
}
