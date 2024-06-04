using ASPNETCoreWebAPI.Contracts;
using ASPNETCoreWebAPI.Services;
using ASPNETCoreWebAPI.Validation;
using Carter;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace ASPNETCoreWebAPI.Endpoints;

public class MathEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithOpenApi();

        api.MapPost("/calculation/{key:int}", Calculation)
            .Produces<CalculationResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status422UnprocessableEntity)
            .WithSummary("Výpočet")
            .WithDescription("Popis výpočtu obsahuje súbor README.md");
    }

    private async Task<IResult> Calculation(
        [FromRoute] int key,
        [Validate][FromBody] CalculationRequest request,
        IPublisherService publisherService, //IBus massTransitBus,
        IMemoryCache memoryCache)
    {
        const double DEFAULT_VALUE = 2d;

        var result = new CalculationResponse
        {
            InputValue = request.Input,
            ComputedValue = DEFAULT_VALUE
        };

        var memoryOptions = new MemoryCacheEntryOptions
        {
            Priority = CacheItemPriority.NeverRemove
        };

        Console.WriteLine($"You requested key:{key}, looking in cache...");

        if (memoryCache.TryGetValue(key, out CalculationResponse? value))
        {
            Console.WriteLine($"Found key:{key}, in the cache!");

            result.PreviousValue = value?.ComputedValue;
            result.Created = value?.Created ?? DateTime.UtcNow;

            if (value?.Created.AddSeconds(15) > DateTime.UtcNow)
            {
                // output hodnota je tretia odmocnina z prirodzeného logaritmu hodnoty input zo vstupu podelenej hodnotou z globálneho storage
                Console.WriteLine($"Computing asynchronously value from input:{request.Input}...");
                result.ComputedValue = await Task.FromResult(Math.Cbrt(Math.Log(Convert.ToDouble(request.Input))) / value.ComputedValue);
            }
            else
            {
                // ak key už v globálnom storage existuje a je starší ako 15 sekúnd od posledného zápisu, nastavte value na hodnotu 2
                Console.WriteLine($"The existing value is older than 15s.");
                result.ComputedValue = DEFAULT_VALUE;
            }
        }
        else
        {
            // ak key poskytnutý cez API nie je nájdený v globálnom key-value storage tak tento <key; value> vytvorte a nastavte value na 2
            Console.WriteLine($"Could not find key:{key} in the cache!");
            result.ComputedValue = DEFAULT_VALUE;
            result.Created = DateTime.UtcNow;
        }

        memoryCache.Set(key, result, memoryOptions);

        // výsledok výpočtu pošlite na vami vytvorenú queue v RabbitMQ
        await publisherService.SendMessgaes(result);    // varianta č.1
        //await massTransitBus.Publish(result);         // varianta č.2

        // následne ju vráťte ako response z endpointu vo formáte JSON
        return TypedResults.Ok(result);
    }
}
