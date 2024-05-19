namespace ASPNETCoreWebAPI.Configurations;

public class RabbitMqConfiguration
{
    public string HostName { get; init; } = null!;
    public int Port { get; init; }
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;
}
