using Carter;
using System.Reflection;

namespace ASPNETCoreWebAPI.Endpoints;

public class HomeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/", (IHostEnvironment env, IConfiguration cfg) => @$"
            Hello {env.ApplicationName}!
            Web API: {DateTime.Now.ToLongDateString()} '{DateTime.Now.ToLongTimeString()}'
            Version: {Assembly.GetEntryAssembly()?.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version}, {File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location)}
            ENV: {env.EnvironmentName}").WithSummary("Web API Info").WithOpenApi();
    }
}
