using ASPNETCoreWebAPI.Contracts;
using ASPNETCoreWebAPI.Validation;
using Carter;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreWebAPI.Endpoints;

public class NewsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/articles")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithOpenApi();

        api.MapGet("/", GetAll)
            .Produces<CalculationResponse>(StatusCodes.Status200OK)
            .WithSummary("Zoznam článkov");

        api.MapGet("/{id:int}", GetDetail)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Detail článku");

        api.MapPost("/", Create)
            .Produces<int>(StatusCodes.Status201Created)
            .WithSummary("Vytvorenie článku");

        api.MapPut("/", Update)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Editácia článku");

        api.MapDelete("/{id:int}", Delete)
            .Produces(StatusCodes.Status204NoContent)
            .WithSummary("Vytvorenie článku");
    }

    private async Task<IResult> GetAll()
    {
        return TypedResults.Ok();
    }

    private async Task<IResult> GetDetail([FromRoute] int id)
    {
        return TypedResults.Ok();
    }

    private async Task<IResult> Create()
    {
        return TypedResults.Created($"/id=0");
    }

    private async Task<IResult> Update()
    {
        return TypedResults.Ok();
    }

    private async Task<IResult> Delete([FromRoute] int id)
    {
        return TypedResults.NoContent();
    }
}
