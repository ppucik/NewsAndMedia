﻿namespace ASPNETCoreWebAPI.Services;

public class ConsumerHostedService : BackgroundService
{
    private readonly IConsumerService _consumerService;

    public ConsumerHostedService(IConsumerService consumerService)
    {
        _consumerService = consumerService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumerService.ReadMessgaes();
    }

    public override void Dispose()
    {
        base.Dispose();
    }
}
