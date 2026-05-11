namespace ASPNETCoreWebAPI.Services;

public interface IPublisherService
{
    Task SendMessges<T>(T message);
}
