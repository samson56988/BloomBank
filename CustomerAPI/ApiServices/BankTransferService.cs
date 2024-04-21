using ApiCaller;
using CustomerAPI.Models.Request;
using CustomerAPI.Models.Response;
using DataBase.Models;
using Newtonsoft.Json;
using System.Text;

namespace CustomerAPI.ApiServices
{
    public interface IBankTransferService
    {         
       Task<BankTransferResponse> InitializeBankTransfer(TransferRequest request);
    }

    public class BankTransferService : IBankTransferService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IConfiguration _configuration;

        public BankTransferService(IHttpClientService httpClientService, IConfiguration configuration)
        {
            _httpClientService = httpClientService;
            _configuration = configuration;
        }

        public async Task<BankTransferResponse> InitializeBankTransfer(TransferRequest request)
        {
            string apiUrl = $"{_configuration.GetValue<string>("MiddleWareService:Base")}{_configuration.GetValue<string>("MiddleWareService:TransferRequest")}";

            var headers = new Dictionary<string, string>();

            headers.Add("X-API-Key", _configuration.GetValue<string>("MiddleWareService:ApiKey"));

            var jsonContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClientService.PostAsync(apiUrl, jsonContent, headers);

            string responseBody = await response.Content.ReadAsStringAsync();

            var transferResponse = JsonConvert.DeserializeObject<BankTransferResponse>(responseBody);

            return transferResponse;
        }
    }
}
