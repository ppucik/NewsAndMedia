using ASPNETCoreWebAPI.Configurations;
using ASPNETCoreWebAPI.Services;

namespace ASPNETCoreWebAPI.Extensions;

public static class RabbitMqExtension
{
    public static void AddRabbitMqService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqConfiguration>(a => configuration.GetSection(nameof(RabbitMqConfiguration)).Bind(a));
        services.AddSingleton<IRabbitMqService, RabbitMqService>();
    }
}
