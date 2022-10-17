using GeekShopping.MessageBus;

namespace GeekShopping.PaymentApi.RabbitMQSende
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
