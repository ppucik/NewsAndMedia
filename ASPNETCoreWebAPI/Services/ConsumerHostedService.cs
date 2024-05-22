namespace ASPNETCoreWebAPI.Services;

public class ConsumerHostedService : BackgroundService
{
    private readonly IConsumerService _consumerService;

    public ConsumerHostedService(IConsumerService consumerService)
    {
        _consumerService = consumerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        //stoppingToken.ThrowIfCancellationRequested();

        await _consumerService.ReadMessgaes();
    }
}
