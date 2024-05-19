using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ASPNETCoreWebAPI.Extensions;

public static class SwaggerExtension
{
    public static void AddSwagger(this IServiceCollection services, string version = "v1")
    {
        var asm = Assembly.GetExecutingAssembly().GetName();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = asm.Name,
                Description = $"Version: {asm.Version?.ToString()}, Date: {File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location)}",
                Version = version
            });
        });
    }
}
