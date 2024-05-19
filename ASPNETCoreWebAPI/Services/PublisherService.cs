using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ASPNETCoreWebAPI.Services;

public class PublisherService : IPublisherService, IDisposable
{
    private readonly ILogger _logger;
    private readonly IModel _model;
    private readonly IConnection _connection;

    public PublisherService(IRabbitMqService rabbitMqService, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<PublisherService>();

        _connection = rabbitMqService.CreateChannel();
        _model = _connection.CreateModel();
    }

    public async Task SendMessgaes<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        _model.BasicPublish(RabbitMqService.DEMO_EXCHANGE_NAME, string.Empty, basicProperties: null, body: body);

        _logger.LogInformation($"Publisher send content {DateTime.Now}");

        await Task.CompletedTask;
    }

    public void Dispose()
    {
        if (_model.IsOpen)
            _model.Close();

        if (_connection.IsOpen)
            _connection.Close();
    }
}
