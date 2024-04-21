using CustomerAPI.Models.Response;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using CustomerAPI.Services.TransferServices;

namespace CustomerAPI.NewFolder
{
    public class RabbitMQConsumerService : BackgroundService
    {
        private readonly ILogger<RabbitMQConsumerService> _logger;
        private readonly IModel _channel;
        private readonly ITransferService _transferService;

        public RabbitMQConsumerService(ILogger<RabbitMQConsumerService> logger,ITransferService transferService)
        {
            _logger = logger;
            _transferService = transferService;

            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                Port = 5672
            };

            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();

            _channel.QueueDeclare(queue: "bank_transfer_response_queue",
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                    _logger.LogInformation($"[x] Received message: {messageBody}");

                    
                    var responsePayload = JsonSerializer.Deserialize<BankTransferApiResponse>(messageBody);
                    ProcessMessage(responsePayload);
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error processing message: {ex.Message}");
                }
            };

            _channel.BasicConsume(queue: "bank_transfer_response_queue",
                                  autoAck: false,
                                  consumer: consumer);

            _logger.LogInformation("RabbitMQ message consumer started.");

            return Task.CompletedTask;
        }

        private void ProcessMessage(BankTransferApiResponse response)
        {
            
            _logger.LogInformation($"Processing BankTransferApiResponse: {response}");
        }

        public override void Dispose()
        {
            _channel.Close();
            _channel.Dispose();
            base.Dispose();
        }
    }
}
