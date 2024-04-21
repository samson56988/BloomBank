using ApiCaller;
using CustomerAPI.ApiServices;
using CustomerAPI.Helpers;
using CustomerAPI.Mapper;
using CustomerAPI.NewFolder;
using CustomerAPI.Services;
using CustomerAPI.Services.AuthenticationServices;
using CustomerAPI.Services.TransferServices;
using DataBase;
using Domain.CustomerDomains.Services.Implementations;
using Domain.CustomerDomains.Services.Interfaces;
using Domain.RepositoryDomain;
using Domain.TransferDomain;
using Library.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Publisher;
using RabbitMQ.Client;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
builder.Services.AddScoped<ICustomerService,CustomerService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<IBankTransferService, BankTransferService>();
builder.Services.AddScoped<ITransferService, TransferService>();
builder.Services.AddScoped<ITransferRepository, TransferRepository>();
builder.Services.AddSingleton<UserTokenGenerator>();
builder.Services.AddSingleton<ConnectionFactory>(provider =>
{
    return new ConnectionFactory()
    {
        HostName = "localhost",
        UserName = "guest",
        Password = "guest",
        Port = 5672
    };
});
builder.Services.AddScoped<IModel>(provider =>
{
    var connectionFactory = provider.GetRequiredService<ConnectionFactory>();
    var connection = connectionFactory.CreateConnection();
    return connection.CreateModel();
});


builder.Services.AddHostedService<RabbitMQConsumerService>();


builder.Services.AddDbContext<BloomDatabaseContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseConnection"));
});

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidAudience = builder.Configuration.GetSection("AuthSettings:Audience").Value,
        ValidIssuer = builder.Configuration.GetSection("AuthSettings:Issuer").Value,
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AuthSettings:Key").Value)),
        ValidateIssuerSigningKey = true
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "BloomBankApi",
        Version = "v1",
        Description = "Authentication & Authorization"
    });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
