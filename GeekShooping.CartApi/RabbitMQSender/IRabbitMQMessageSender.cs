using GeekShopping.MessageBus;

namespace GeekShooping.CartApi.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, string queueName);
    }
}
