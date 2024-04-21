namespace MiddleWareDomain.Models
{
    public class CorprateWalletAccountSetup
    {
        public string AccountNumber { get; set; }
        public string BusinessKey { get; set; }
        public string CurrencyCode { get; set; }
        public string AccountName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateDeactivated { get; set; }
        public string BusinessName {  get; set; }
        public bool IsDeactivated { get; set; }
    }


    public class CreateWalletAccountDto
    {
        public string AccountNumber { get; set; }
        public string CurrencyCode { get; set; }
        public string AccountName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateDeactivated { get; set; }
        public string BusinessName { get; set; }
        public bool IsDeactivated { get; set; }
    }
}
