namespace MiddleWareAPI.Services
{
    public interface IAccountConfiguration
    {
        decimal ConfigureAccount(string currencyCode, string accountType);
       
    }
    public class AccountConfiguration : IAccountConfiguration
    {
        public decimal ConfigureAccount(string currencyCode, string accountType)
        {
            decimal TransactionLimit = 0;

            if(currencyCode == "NGN")
            {
                if(accountType == "Savings")
                {
                    TransactionLimit = 500000;
                }
                else if(accountType == "Current")
                {
                    TransactionLimit = 70000000;
                }
                else
                {
                    TransactionLimit = 100000000;
                }
            }
            else 
            {
                if (accountType == "Savings")
                {
                    TransactionLimit = 500000;
                }
                else if (accountType == "Current")
                {
                    TransactionLimit = 70000000;
                }
                else
                {
                    TransactionLimit = 100000000;
                }
            }

            return TransactionLimit;
        }
    }

    
}
