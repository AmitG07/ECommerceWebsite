using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Products.Rabbitmq
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly ILogger<RabbitMqService> _logger;

        public RabbitMqService(IConfiguration configuration, ILogger<RabbitMqService> logger)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "myuser",
                Password = "myuser1",
                Port = 5672,
            };
            _logger = logger;
        }
        public void SendingMessage<T>(T message)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();
                using var channel = connection.CreateModel();

                channel.ExchangeDeclare(
                    "ProductExchange",
                    ExchangeType.Direct);

                channel.QueueDeclare(
                    queue: "product_Queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                channel.QueueBind(
                    queue: "product_Queue",
                    exchange: "ProductExchange",
                    routingKey: "product_route");

                var jsonString = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(jsonString);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true; // Ensure message persistence

                channel.BasicPublish(
                exchange: "ProductExchange",
                routingKey: "product_route",
                basicProperties: properties,
                body: body);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error sending message: {ex.Message}");
            }
        }
    }
}
