namespace ASPNETCoreWebAPI.Services;

public interface IPublisherService
{
    Task SendMessgaes<T>(T message);
}
