using Microsoft.JSInterop;

namespace CustomerPortal.Helpers
{
    public interface ITokenService
    {
        Task SetTokenAsync(string token);
        Task<string> GetTokenAsync();
        Task<bool> TokenExistsAsync();
        Task ClearToken();
    }

    public class TokenService : ITokenService
    {
        private const string TokenKey = "";
        private readonly IJSRuntime _jsRuntime;

        public TokenService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetTokenAsync(string token)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
        }

        public async Task<string> GetTokenAsync()
        {
            return await _jsRuntime.InvokeAsync<string>("localStorage.getItem", TokenKey);
        }

        public async Task<bool> TokenExistsAsync()
        {
            var token = await GetTokenAsync();
            return !string.IsNullOrWhiteSpace(token);
        }

        public async Task ClearToken()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        }
    }
}
