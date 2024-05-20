using ASPNETCoreWebAPI.Endpoints;
using ASPNETCoreWebAPI.Extensions;
using ASPNETCoreWebAPI.Middleware;
using ASPNETCoreWebAPI.Repositories;
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

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
builder.Services.AddMemoryCache();
builder.Services.AddSwagger();

// Add RabbitMQ services
builder.Services.AddRabbitMqService(builder.Configuration);
builder.Services.AddScoped<IPublisherService, PublisherService>();
builder.Services.AddSingleton<IConsumerService, ConsumerService>();
builder.Services.AddHostedService<ConsumerHostedService>();

// Global exception handling
builder.Services.AddProblemDetails();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options => { options.DisplayRequestDuration(); });
}

app.UseHttpsRedirection();

// Add Middlewares
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

// Map API endpoints
app.MapSampleEndpoint();

// Migrate latest database changes during startup
app.ApplyMigrations();

app.Run();
