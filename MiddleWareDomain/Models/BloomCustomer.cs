

namespace MiddleWareDomain.Models
{
    public class BloomCustomer
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
        public string AccountType { get; set; } // e.g., Current, Savings, Foreign
        public string IdentificationType { get; set; }
        public string IdentificationNumber { get; set; }
        public decimal TransactionLimit { get; set; }
        public DateTime DateOnboarded { get; set; }
        public decimal AccountBalance { get; set; }
        public string CifNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool HasPND { get; set; }
        public string CurrencyCode { get; set; } // ISO currency code (e.g., NGN for Naira, USD for US Dollar)
        public string AccountName { get; set; }
    }

}
