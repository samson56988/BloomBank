using ApiCaller;
using CustomerAPI.Models.Request;
using CustomerAPI.Models.Response;
using Newtonsoft.Json;
using System.Text;

namespace CustomerAPI.ApiServices
{
    public interface IAccountService
    {
        Task<GetAccountDetailsResponse> GetAccountDetailsAsync(string accountId);

        Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest customerRequest);
    }
    public class AccountService : IAccountService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;

        public AccountService(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
        }

        public async Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest customerRequest)
        {
            try
            {
                string apiUrl = $"{_configuration.GetValue<string>("MiddleWareService:Base")}{_configuration.GetValue<string>("MiddleWareService:CreateCustomer")}";

                var headers = new Dictionary<string, string>();

                headers.Add("X-API-Key", _configuration.GetValue<string>("MiddleWareService:ApiKey"));

                var jsonContent = new StringContent(JsonConvert.SerializeObject(customerRequest), Encoding.UTF8, "application/json");

                var response = await _httpClientService.PostAsync(apiUrl, jsonContent, headers);

                string responseBody = await response.Content.ReadAsStringAsync();

                var createdAccount = JsonConvert.DeserializeObject<CreateCustomerResponse>(responseBody);

                return createdAccount;
            }
            catch
            {
                throw;                                                                                                                                                              
            }
        }

        public async Task<GetAccountDetailsResponse> GetAccountDetailsAsync(string accountId)
        {
            try
            {
                string apiUrl = $"{_configuration.GetValue<string>("MiddleWareService:Base")}{_configuration.GetValue<string>("MiddleWareService:GetAccountDetails")}{accountId}";

                var headers = new Dictionary<string, string>();

                headers.Add("X-API-Key", _configuration.GetValue<string>("MiddleWareService:ApiKey"));

                var response = await _httpClientService.GetAsync(apiUrl, headers);

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();

                    var getAccount = JsonConvert.DeserializeObject<GetAccountDetailsResponse>(responseBody);

                    return getAccount;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
           
        }
    }
}
