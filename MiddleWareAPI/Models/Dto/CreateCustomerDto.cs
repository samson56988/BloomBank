namespace MiddleWareAPI.Models.Dto
{
    public class CreateCustomerDto
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
        public string AccountType { get; set; }
        public string IdentificationType { get; set; }
        public string IdNo { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DateOnboarded { get; set; }
        public string BVN { get; set; }
        public bool HasBvn { get; set; }
        public string CurrencyCode { get; set; }
        public string AccountName { get; set; }
    }
}
