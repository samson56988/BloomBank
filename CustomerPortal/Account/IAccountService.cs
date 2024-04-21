using CustomerPortal.Models.Request;
using CustomerPortal.Models.Response;

namespace CustomerPortal.Account
{
    public interface IAccountService
    {
        Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest request);

        Task<CreatePasswordResponse> CreatePassword(CreatePasswordRequest request);

        Task<GetAccountDetailsResponse> GetAccountDetails(string accountNumber);

        Task<LoginResponse> Login(string accountNo, string password);
    }
}
