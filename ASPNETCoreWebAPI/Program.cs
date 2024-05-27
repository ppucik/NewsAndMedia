using ASPNETCoreWebAPI.Endpoints;
using ASPNETCoreWebAPI.Extensions;
using ASPNETCoreWebAPI.Repositories;
using ASPNETCoreWebAPI.Repositories.Interceptors;
using ASPNETCoreWebAPI.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add validation
builder.Services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Singleton);

// JSON serialization
builder.Services.Configure<JsonOptions>(options => new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true });

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

// Add RabbitMQ services
builder.Services.AddRabbitMqService(builder.Configuration);
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.Services.AddHostedService<ConsumerHostedService>();

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

// Map API endpoints
app.MapSampleEndpoint();

// Migrate latest database changes during startup
app.ApplyMigrations();

app.Run();
