using CustomerPortal.Models.Request;
using CustomerPortal.Models.Response;
using System.Net.Http.Json;

namespace CustomerPortal.Transfer
{
    public interface ITransferService
    {
       Task<TransferServiceResponse> BankTransfer(BankTransferRequest request);

       Task<TransactionHistory> TransactionHistory(string accountNo);
    }

    public class TransferService : ITransferService
    {
        private readonly HttpClient _httpClient;

        public TransferService(HttpClient httpClient) 
        { 
          _httpClient = httpClient;     
        }

        public async Task<TransferServiceResponse> BankTransfer(BankTransferRequest request)
        {
            string api = $"{Settings.BankTransfer}";

            try
            {
                var response = await _httpClient.PostAsJsonAsync(api, request);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<TransferServiceResponse>();
                    return responseData;
                }
                else
                {
                    var responseData = await response.Content.ReadFromJsonAsync<TransferServiceResponse>();
                    return responseData;
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async Task<TransactionHistory> TransactionHistory(string accountNo)
        {
            string apiUrl = $"{Settings.BankTransferHistory}{accountNo}";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadFromJsonAsync<TransactionHistory>();
                    return responseData;
                }
                else
                {
                    var responseData = await response.Content.ReadFromJsonAsync<TransactionHistory>();
                    return responseData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
