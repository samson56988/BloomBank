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
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IModel _channel;
        private Timer _timer;

        public RabbitMQConsumerService(ILogger<RabbitMQConsumerService> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // Initialize RabbitMQ connection and channel
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

            // Setup timer to trigger message consumption every 30 seconds
            _timer = new Timer(ConsumeMessages, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping RabbitMQ message consumer...");

            // Dispose of timer and RabbitMQ channel
            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();

            _channel?.Close();
            _channel?.Dispose();

            await base.StopAsync(cancellationToken);
        }

        private void ConsumeMessages(object state)
        {
            try
            {
                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += async (model, ea) =>
                {
                    try
                    {
                        var messageBody = Encoding.UTF8.GetString(ea.Body.ToArray());
                        _logger.LogInformation($"[x] Received message: {messageBody}");

                        var responsePayload = JsonSerializer.Deserialize<BankTransferApiResponse>(messageBody);
                        await ProcessMessageAsync(responsePayload);

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
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error starting message consumer: {ex.Message}");
            }
        }

        private async Task ProcessMessageAsync(BankTransferApiResponse response)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var transferService = scope.ServiceProvider.GetRequiredService<ITransferService>();

                await transferService.BankTransferResponse(response);
            }

            _logger.LogInformation($"Processed BankTransferApiResponse: {response}");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            return Task.CompletedTask;
        }
    }
}
