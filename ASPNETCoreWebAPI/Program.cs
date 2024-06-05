using ASPNETCoreWebAPI.Configurations;
using ASPNETCoreWebAPI.Extensions;
using ASPNETCoreWebAPI.Health;
using ASPNETCoreWebAPI.Repositories;
using ASPNETCoreWebAPI.Repositories.Interceptors;
using ASPNETCoreWebAPI.Services;
using Carter;
using FluentValidation;
using HealthChecks.UI.Client;
//using MassTransit;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Configurations
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var rabbitMqConfiguration = new RabbitMqConfiguration();
builder.Configuration.GetSection(nameof(RabbitMqConfiguration)).Bind(rabbitMqConfiguration);

// Add validation
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

// JSON serialization
builder.Services.Configure<JsonOptions>(options => new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true });

// Add MediatR
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(assembly));

// Global exception handling
builder.Services.AddGlobalErrorHandler();

// Register interceptors
builder.Services.AddSingleton<UpdateAuditableInterceptor>();

// Add services to the container
builder.Services.AddDbContext<AppDbContext>((sp, options) => options
    .UseNpgsql(connectionString)
    .AddInterceptors(sp.GetRequiredService<UpdateAuditableInterceptor>())
);
builder.Services.AddMemoryCache();
builder.Services.AddSwagger();
builder.Services.AddCarter();

// Add RabbitMQ services - varianta č.1
builder.Services.AddRabbitMqService(builder.Configuration);
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.Services.AddHostedService<ConsumerHostedService>();

//// Add MassTransit services (RabbitMQ) - varianta č.2
//builder.Services.AddMassTransit(busConfiguration =>
//{
//    busConfiguration.SetKebabCaseEndpointNameFormatter();
//    busConfiguration.AddConsumers(assembly);
//    busConfiguration.UsingRabbitMq((context, config) =>
//    {
//        config.Host(rabbitMqConfiguration.GetConnectionString());
//        config.ConfigureEndpoints(context);
//    });
//});

// Add Health checks monitoring
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AppDbContext>()
    .AddNpgSql(connectionString!)
    .AddRabbitMQ(rabbitMqConfiguration.GetConnectionString())
    .AddCheck<DatabaseHealthCheck>("custom-sql", HealthStatus.Unhealthy);
builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.DisplayRequestDuration(); });
}

// > dotnet dev-certs https --check --trust
app.UseHttpsRedirection();

// Add Middlewares
app.UseGlobalErrorHandlerMiddleware();

// Map API 
app.MapCarter();

app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();

// Migrate latest database changes during startup
app.ApplyMigrations();

app.Run();
