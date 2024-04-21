
using MiddleWareAPI.Models.Dto;
using MiddleWareDomain.Models;

namespace MiddleWareAPI.Services.AccountService
{
    public interface IAccountService
    {
        Task<Tuple<object, string>> CreateAccountService(CreateCustomerDto customer);

        Task<Tuple<object, string>> GetAccountDetails(string AccountNO);

        Task<Tuple<object, string>> SetAccountOnPND(string AccountNO, bool hasPND);

        Task<Tuple<object, string>> CreateCorporateWalletAccount(CreateWalletAccountDto corprateWallet);

        Task<Tuple<object, string>> GetCorporateWalletAccount(string businessKey);

        Task<Tuple<object, string>> ActivateCorporateWalletAccount(string accountNumber);

        Task<Tuple<object, string>> DeactivateCorporateWalletAccount(string accountNumber);

        Task<Tuple<object, string>> CreateWalletAccount(WalletAccountDto account);

        Task<Tuple<object, string>> CreateVirtualCard(VirtualCardDetailsDto account);

        Task<Tuple<object, string>> BlockVirtualCard(string customer, string cardId);

        Task<Tuple<object, string>> ActivateVirtualCard(string customerId, string cardPin, string cardId);

        Task<Tuple<object, string>> GetCustomerActiveCards(string customerId);

        Task<Tuple<object, string>> FundAccount(string customerId, decimal amount);
    }
}
