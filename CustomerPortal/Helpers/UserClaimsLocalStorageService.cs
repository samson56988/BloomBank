using CustomerPortal.Models.Dtos;
using Library.Security;
using Microsoft.JSInterop;
using System.Text.Json;

namespace CustomerPortal.Helpers
{
    public interface IUserClaimsLocalStorageService
    {
        Task SetAccountInfoAsync(AccountInfo accountInfo);

        Task<AccountInfo> GetAccountInfoAsync();

        Task RemoveAccountInfoAsync();
    }


    public class UserClaimsLocalStorageService:IUserClaimsLocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public UserClaimsLocalStorageService(IJSRuntime runtime)
        {
            _jsRuntime = runtime;
        }

        public async Task SetAccountInfoAsync(AccountInfo accountInfo)
        {
            string serializedAccountInfo = JsonSerializer.Serialize(accountInfo);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "accountInfo", serializedAccountInfo);
        }


        public async Task<AccountInfo> GetAccountInfoAsync()
        {
            string accountData = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "accountInfo");
            if (!string.IsNullOrEmpty(accountData))
            {
                return JsonSerializer.Deserialize<AccountInfo>(accountData);
            }
            return null;
        }

        public async Task RemoveAccountInfoAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "accountInfo");
        }
    }
}
