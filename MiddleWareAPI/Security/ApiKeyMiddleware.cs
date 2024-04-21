namespace MiddleWareAPI.Security
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string ApiKeyName = "X-API-Key";
        private readonly IConfiguration _configuration;

        public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            string apiKey = context.Request.Headers[ApiKeyName];

            if (!string.IsNullOrEmpty(apiKey) && apiKey == _configuration.GetValue<string>("AuthSettings:APIKEY"))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: API key is missing or invalid");
            }
        }
    }
}
