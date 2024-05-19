using RabbitMQ.Client;

namespace ASPNETCoreWebAPI.Services;

public interface IRabbitMqService
{
    IConnection CreateChannel();
}
