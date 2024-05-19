using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ASPNETCoreWebAPI.Middleware;

/// <summary>
/// Global Exception Handling Middleware <see href="https://datatracker.ietf.org/doc/html/rfc7807"/>
/// </summary>
public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            string problemTitle = "Server Error";
            string problemType = "https://tools.ietf.org/html/";
            int statusCode = StatusCodes.Status500InternalServerError;

            switch (ex)
            {
                case BadRequestException _:
                    problemTitle = "Bad Request";
                    problemType += "rfc7231#section-6.5.1";
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case NotFoundException _:
                    problemTitle = "Not Found";
                    problemType += "rfc7231#section-6.5.4";
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                default:
                    problemType += "rfc7231#section-6.6.1";
                    break;
            }

            _logger.LogError(ex, $"{problemTitle}: {context.Request.Path} call failed | TraceId: {context.TraceIdentifier} | Exception: {ex.Message}");

            var problemDetails = new ProblemDetails()
            {
                Title = problemTitle,
                Type = problemType,
                Instance = context.Request.Path,
                Status = statusCode,
                Detail = ex.Message + (ex.InnerException != null ? $"({ex.InnerException?.Message})" : string.Empty),
                Extensions = { { "traceId", context.TraceIdentifier } }
            };

            string json = JsonSerializer.Serialize(problemDetails);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(json);
        }
    }
}

#region Exceptions

/// <summary>
/// The 400 (Bad Request) status code indicates that the server cannot or 
/// will not process the request due to something that is perceived to be 
/// a client error(e.g., malformed request syntax, invalid request message 
/// framing, or deceptive request routing).
/// <see href="https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"/>
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) { }
}

/// <summary>
/// The 404 (Not Found) status code indicates that the origin server did
/// not find a current representation for the target resource or is not
/// willing to disclose that one exists.
/// <see href="https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4"/>
/// </summary>
public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}

#endregion
