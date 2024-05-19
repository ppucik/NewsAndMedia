using ASPNETCoreWebAPI.Configurations;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace ASPNETCoreWebAPI.Services;

public class RabbitMqService : IRabbitMqService
{
    public const string DEMO_QUEUE_NAME = "demo.queue";
    public const string DEMO_EXCHANGE_NAME = "demo.exchange";

    private readonly RabbitMqConfiguration _configuration;

    public RabbitMqService(IOptions<RabbitMqConfiguration> options)
    {
        _configuration = options.Value;
    }
    public IConnection CreateChannel()
    {
        // configuration from appsettings.json
        ConnectionFactory connection = new ConnectionFactory()
        {
            HostName = _configuration.HostName,
            Port = _configuration.Port,
            UserName = _configuration.Username,
            Password = _configuration.Password
        };

        connection.DispatchConsumersAsync = true;

        // create connection
        var channel = connection.CreateConnection();

        return channel;
    }
}
