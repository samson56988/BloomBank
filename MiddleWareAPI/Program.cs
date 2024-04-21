using EmailProcessor.EmailServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using MiddleWareAPI.DatabaseLogic;
using MiddleWareAPI.Security;
using MiddleWareAPI.Services;
using MiddleWareAPI.Services.AccountService;
using MiddleWareAPI.Services.TransferService;
using Publisher;
using Hangfire;
using Hangfire.PostgreSql;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.





// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IDatabaseLogic, DatabaseLogic>();
builder.Services.AddScoped<IRabbitMQPublisher, RabbitMQPublisher>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IMailJetService, MailJetService>();
builder.Services.AddScoped<IAccountConfiguration, AccountConfiguration>();
builder.Services.AddAuthorization();
builder.Services.AddControllers();


builder.Host.UseSerilog((hostingContext, loggerConfiguration) => loggerConfiguration
    .MinimumLevel.Information()
    .WriteTo.Console()
    .WriteTo.File(configuration["Log:Path"], rollingInterval: RollingInterval.Day) 
);



var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();



app.MapControllers();

app.Run();
