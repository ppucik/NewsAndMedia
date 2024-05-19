using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ASPNETCoreWebAPI.Services;

public class ConsumerService : IConsumerService, IDisposable
{
    private readonly ILogger _logger;
    private readonly IModel _model;
    private readonly IConnection _connection;

    public ConsumerService(IRabbitMqService rabbitMqService, ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ConsumerService>();

        _connection = rabbitMqService.CreateChannel();

        string queueName = RabbitMqService.DEMO_QUEUE_NAME;
        string exchangeName = RabbitMqService.DEMO_EXCHANGE_NAME;

        _model = _connection.CreateModel();
        _model.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
        _model.ExchangeDeclare(exchangeName, ExchangeType.Fanout, durable: true, autoDelete: false);
        _model.QueueBind(queueName, exchangeName, string.Empty);
    }

    public async Task ReadMessgaes()
    {
        var consumer = new AsyncEventingBasicConsumer(_model);

        consumer.Received += async (ch, ea) =>
        {
            // received message
            var body = ea.Body.ToArray();
            var content = System.Text.Encoding.UTF8.GetString(body);

            // handle the received message
            HandleMessage(content);

            await Task.CompletedTask;

            _model.BasicAck(ea.DeliveryTag, false);
        };

        _model.BasicConsume(RabbitMqService.DEMO_QUEUE_NAME, false, consumer);

        await Task.CompletedTask;
    }

    private void HandleMessage(string content)
    {
        _logger.LogInformation($"Consumer received: {content}");
    }

    public void Dispose()
    {
        if (_model.IsOpen)
            _model.Close();

        if (_connection.IsOpen)
            _connection.Close();
    }
}
