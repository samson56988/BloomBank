using CustomerPortal;
using CustomerPortal.Account;
using CustomerPortal.Helpers;
using Library.Security;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<IAccountService,AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();    
builder.Services.AddScoped<IUserClaimsLocalStorageService, UserClaimsLocalStorageService>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7165/") });
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 2000;
    config.SnackbarConfiguration.HideTransitionDuration = 400;
    config.SnackbarConfiguration.ShowTransitionDuration = 400;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Outlined;
});



await builder.Build().RunAsync();
