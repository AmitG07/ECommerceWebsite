namespace Products.Rabbitmq
{
    public interface IRabbitMqService
    {
        public void SendingMessage<T>(T message);
    }
}
