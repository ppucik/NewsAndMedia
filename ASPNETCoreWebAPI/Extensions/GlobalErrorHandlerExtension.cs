using ASPNETCoreWebAPI.Middleware;

namespace ASPNETCoreWebAPI.Extensions;

public static class GlobalErrorHandlerExtension
{
    public static void AddGlobalErrorHandler(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
    }

    public static IApplicationBuilder UseGlobalErrorHandlerMiddleware(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<GlobalExceptionHandlingMiddleware>();
}
