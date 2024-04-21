using CustomerPortal.Models.Request;
using CustomerPortal.Models.Response;
using System.Net.Http.Json;

namespace CustomerPortal.Account
{
    public class AccountService : IAccountService
    {
        public readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest request)
        {

            string api = $"{Settings.CreateAccount}";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(api, request);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<CreateAccountResponse>();
                    return responseData;
                }
                else
                {
                    var responseData = await response.Content.ReadFromJsonAsync<CreateAccountResponse>();
                    return responseData;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<CreatePasswordResponse> CreatePassword(CreatePasswordRequest request)
        {
            string api = $"{Settings.CreateAuthentication}";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(api, request);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<CreatePasswordResponse>();
                    return responseData;
                }
                else
                {
                    var responseData = await response.Content.ReadFromJsonAsync<CreatePasswordResponse>();
                    return responseData;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                throw;
            }
        }

        public async Task<GetAccountDetailsResponse> GetAccountDetails(string accountNumber)
        {
            string apiUrl = $"{Settings.ValidateAccountNumber}{accountNumber}"; 

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<GetAccountDetailsResponse>();
                    return responseData;
                }
                else
                {
                    var responseData = await response.Content.ReadFromJsonAsync<GetAccountDetailsResponse>();
                    return responseData;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; 
            }
        }

        public async Task<LoginResponse> Login(string accountNo, string password)
        {
            string apiUrl = Settings.CustomerLogin;

            LoginRequest request = new LoginRequest();
            request.AccountNo = accountNo;
            request.Password = password;

            try
            {
                var response = await _httpClient.PostAsJsonAsync(apiUrl, request);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    return responseData;
                }
                else
                {
                    var responseData = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    return responseData;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
