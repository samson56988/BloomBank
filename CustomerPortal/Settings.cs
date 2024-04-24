namespace CustomerPortal
{
    public class Settings
    {
        public const string CreateAccount = "api/Account/CreateAccount";

        public const string CreateAuthentication = "api/Auth/CreateLogin";

        public const string ValidateAccountNumber = "api/Account/GetAccountDetail?accountId=";

        public const string CustomerLogin = "api/Auth/Login";

        public const string BankTransfer = "api/Transfer/Transfer";

        public const string BankTransferHistory = "api/Transfer/TransactionHistory?account=";
    }
}
