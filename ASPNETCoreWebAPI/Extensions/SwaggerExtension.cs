using Microsoft.OpenApi.Models;
using System.Reflection;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

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
                Title = "News and Media Web API",
                Description = $"Version: {asm.Version?.ToString()}, Date: {File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location)}",
                Version = version
            });

            // Include XML comments
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, $"{asm.Name}.xml");
            options.IncludeXmlComments(xmlFilePath, true);
            options.IncludeXmlCommentsWithRemarks(xmlFilePath, true);
            options.IncludeXmlCommentsFromInheritDocs(includeRemarks: true, excludedTypes: typeof(string));
        });
    }
}
