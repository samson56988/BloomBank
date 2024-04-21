namespace MiddleWareDomain.Models
{
    public class WalletAccount
    {
        public string WalletNo { get; set; }
        public string CoprateAccountNo { get; set; }
        public string CoprateAccountName { get; set; }
        public decimal AccountBalance { get; set; }
        public string CoprateAccountBusinessKey { get; set; }
        public DateTime DateCreated { get; set; }
        public string CurrencyCode { get; set; }
        public string WalletAccountName { get; set; }
    }

    public class WalletAccountDto
    {
        public string CoprateAccountNo { get; set; }
        public string CoprateAccountBusinessKey { get; set; }
        public DateTime DateCreated { get; set; }
        public string WalletAccountName { get; set; }
    }
}
