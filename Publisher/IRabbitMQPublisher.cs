using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    public interface IRabbitMQPublisher
    {
        bool PublishBankTransfer(string queueName, string message);
        bool PublishBankTransferResponse(string queueName, string message); 
    }

    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQPublisher()
        {
            _factory = new ConnectionFactory()
            {
                HostName = ConfigurationManager.AppSettings["RabbitMQHostName"],
                UserName = ConfigurationManager.AppSettings["RabbitMQUserName"],
                Password = ConfigurationManager.AppSettings["RabbitMQPassword"]
            };
        }

        public bool PublishBankTransfer(string queueName, string message)
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

                var conn = factory.CreateConnection();
                using var channel = conn.CreateModel();
                channel.QueueDeclare(queueName, false, false, false, null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", queueName, null, body);

                return true; 
                
            }
            catch
            {
                return false;
            }
        }

        public bool PublishBankTransferResponse(string queueName, string message)
        {
            try
            {
                ConnectionFactory factory = new ConnectionFactory();
                factory.Uri = new Uri("amqp://guest:guest@localhost:5672");

                var conn = factory.CreateConnection();
                using var channel = conn.CreateModel();
                channel.QueueDeclare(queueName, false, false, false, null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", queueName, null, body);

                return true;

            }
            catch
            {
                return false;
            }
        }

    }
}
