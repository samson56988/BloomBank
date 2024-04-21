namespace CustomerAPI.Dtos
{
    public class OnboardCustomerDto
    {
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
        public string IdenitificationNumber { get; set; }
        public string AccountNumber { get; set; }
        public decimal TransactionLimit { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public string ProfileImage { get; set; }
        public DateTime DateOnboarded { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
