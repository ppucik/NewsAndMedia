using ASPNETCoreWebAPI.Contracts;
using MassTransit;
using System.Text.Json;

namespace ASPNETCoreWebAPI.Services;

// RabbitMQ) - varianta č.2

public class MassTransitConsumer : IConsumer<CalculationResponse>
{
    private readonly ILogger<MassTransitConsumer> _logger;

    public MassTransitConsumer(ILogger<MassTransitConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CalculationResponse> context)
    {
        _logger.LogInformation($"{nameof(MassTransitConsumer)} received: {JsonSerializer.Serialize(context.Message).ToString()}");

        return Task.CompletedTask;
    }
}
