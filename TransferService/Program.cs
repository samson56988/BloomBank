using System;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using MiddleWareAPI.DatabaseLogic;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Publisher;
using TransferService;
using EmailProcessor.EmailServices;
using System.Configuration;
using Serilog;
using TransferService.Models;
using System.Text.Json;

namespace RabbitMQConsumer
{
    class Program
    {
        private readonly IDatabaseLogic _databaseLogic;
        private readonly TransferProcessor _transferProcessor;
        private readonly IRabbitMQPublisher _rabbitmqPublisher;

        public Program(IDatabaseLogic databaseLogic, TransferProcessor transferProcessor, IRabbitMQPublisher rabbitmqPublisher)
        {
            _databaseLogic = databaseLogic;
            _transferProcessor = transferProcessor;
            _rabbitmqPublisher = rabbitmqPublisher;
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("RabbitMQ Consumer Bank Transfer started. Press CTRL+C to exit.");

            // Setup DI

            var logPath = ConfigurationManager.AppSettings["logFilePath"];

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDatabaseLogic, DatabaseLogic>()
                .AddSingleton<IRabbitMQPublisher, RabbitMQPublisher>()
                .AddSingleton<IEmailService, EmailService>()
                .AddSingleton<IMailJetService, MailJetService>()
                .AddSingleton<TransferProcessor>() 
                .BuildServiceProvider();

            var program = new Program(
                serviceProvider.GetRequiredService<IDatabaseLogic>(),
                serviceProvider.GetRequiredService<TransferProcessor>(),
                serviceProvider.GetRequiredService<IRabbitMQPublisher>()
            );

            await program.Run();
        }

        private async Task Run()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "bank_transfer_queue",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, eventArgs) =>
                {
                    var body = eventArgs.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    // Deserialize the message into TransferDetail object
                    TransferDetail transferDetail = JsonConvert.DeserializeObject<TransferDetail>(message);

                    BankTransferResponse response = new BankTransferResponse();

                    response.AccountNumber = transferDetail.SenderAccountNumber;
                    response.TransactionRef = transferDetail.TransactionReference;
                    

                    if (transferDetail != null)
                    {
                        Console.WriteLine($"About to process transfer for Name: {transferDetail.PayeeName} PayeeAccountNumber {transferDetail.PayeeAccountNumber} Amount {transferDetail.Amount}");

                        var transfer = await _transferProcessor.Transfer(transferDetail.SenderAccountNumber, transferDetail.PayeeAccountNumber, transferDetail.Amount,transferDetail.Narration);
                        if (transfer)
                        {
                            response.Status = "success";
                            response.ApiResponse = null;
                            response.AccountNumber = transferDetail.SenderAccountNumber;
                            response.TransactionRef = transferDetail.TransactionReference;

                            string serializedresponse = System.Text.Json.JsonSerializer.Serialize(response);

                             _rabbitmqPublisher.PublishBankTransferResponse("bank_transfer_response_queue", serializedresponse);

                            Console.WriteLine($"Successfully Debited Payer Account: {transferDetail.SenderAccountNumber} and Credited Beneficiary Account:  {transferDetail.PayeeAccountNumber}");

                            var getAccountDetails = await _transferProcessor.GetAccountDetails(transferDetail.PayeeAccountNumber);

                            var sendBeneficiaryMail = await _transferProcessor.SendTransactionNotification(transferDetail.PayeeName, getAccountDetails.Email, DateTime.Now.ToString("dd-MM-yyyy"), "Credit", transferDetail.Narration, transferDetail.Amount.ToString(), true);               

                        }
                        else
                        {
                            response.Status = "failed";
                            response.ApiResponse = null;
                            response.AccountNumber = transferDetail.SenderAccountNumber;
                            response.TransactionRef = transferDetail.TransactionReference;
                            string serializedresponse = System.Text.Json.JsonSerializer.Serialize(response);
                            _rabbitmqPublisher.PublishBankTransferResponse("bank_transfer_response_queue", serializedresponse);

                        }
                    }
                    else
                    {
                        Console.WriteLine("Received invalid message: " + message);
                    }

                    Console.WriteLine("Received message: " + $"TransactionId: {transferDetail.Id} , PayeeName: {transferDetail.PayeeName} , AccountNo: {transferDetail.PayeeAccountNumber}");
                };

                channel.BasicConsume(queue: "bank_transfer_queue",
                                     autoAck: true,
                                     consumer: consumer);

                Console.ReadLine();
            }
        }
    }

}

