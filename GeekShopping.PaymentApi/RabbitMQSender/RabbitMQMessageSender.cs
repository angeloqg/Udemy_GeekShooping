using GeekShopping.MessageBus;
using GeekShopping.PaymentApi.Messages;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace GeekShopping.PaymentApi.RabbitMQSende
{
    public class RabbitMQMessageSender : IRabbitMQMessageSender
    {
        private readonly string _hostName;
        private readonly string _password;
        private readonly string _username;
        private IConnection _connection;

        private const string exchangeName = "DirectPaymentUpdateExchange";
        private const string paymentEmailUpdateQueueName = "PaymentEmailUpdateExchange";
        private const string paymentOrderUpdateQueueName = "PaymentOrderUpdateExchange";

        public RabbitMQMessageSender()
        {
            _hostName = "localhost";
            _password = "guest";
            _username = "guest";
        }

        public void SendMessage(BaseMessage message)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();

                channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: false);

                channel.QueueDeclare(paymentEmailUpdateQueueName, false, false, false, null);
                channel.QueueDeclare(paymentOrderUpdateQueueName, false, false, false, null);

                channel.QueueBind(paymentEmailUpdateQueueName, exchangeName, "PaymentEmail");
                channel.QueueBind(paymentOrderUpdateQueueName, exchangeName, "PaymentOrder");
                byte[] body = GetMesssageAsByteArray(message);

                channel.BasicPublish(exchange: exchangeName, "PaymentEmail", basicProperties: null, body: body);
                channel.BasicPublish(exchange: exchangeName, "PaymentOrder", basicProperties: null, body: body);

            }
        }

        private byte[] GetMesssageAsByteArray(BaseMessage message)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize<UpdatePaymentResultMessage>((UpdatePaymentResultMessage)message, options);

            var body = Encoding.UTF8.GetBytes(json);

            return body;
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _username,
                    Password = _password
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                // Log Exceptionm
                throw;
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null) return true;
            CreateConnection();
            return _connection != null;
        }
    }
}
