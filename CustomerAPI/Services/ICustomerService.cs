using CustomerAPI.Dtos;
using CustomerAPI.Models.Request;

namespace CustomerAPI.Services
{
    public interface ICustomerService
    {
        Task<Tuple<object, string>> GetCustomerAccountDetails(string accountNo);

        Task<Tuple<object, string>> CreateAccount(CreateCustomerRequest request);

        Task<Tuple<object, string>> ActivateCustomer(string accountNo);

    }
}
