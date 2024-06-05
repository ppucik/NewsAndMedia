using ASPNETCoreWebAPI.Contracts;
using ASPNETCoreWebAPI.Handlers.Articles;
using ASPNETCoreWebAPI.Validation;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreWebAPI.Endpoints;

public class NewsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/articles")
            .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
            .WithOpenApi();

        api.MapGet("/", GetArticles)
            .Produces<IReadOnlyCollection<ArticleResponse>>(StatusCodes.Status200OK)
            .WithSummary("Zoznam článkov");

        api.MapGet("/{id:int}", GetArticle)
            .Produces<ArticleResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithSummary("Detail článku");

        api.MapPost("/", CreateArticle)
            .Produces(StatusCodes.Status201Created)
            .WithSummary("Zaevidovanie nového článku");

        api.MapPut("/{id:int}", UpdateArticle)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithSummary("Editácia údajov článku");

        api.MapDelete("/{id:int}", DeleteArticle)
            .Produces(StatusCodes.Status204NoContent)
            .WithSummary("Vymazanie článku");
    }

    private async Task<IResult> GetArticles(IMediator mediator)
    {
        var result = await mediator.Send(new GetArticles.Query());

        return TypedResults.Ok(result);
    }

    private async Task<IResult> GetArticle([FromRoute] int id, IMediator mediator)
    {
        var result = await mediator.Send(new GetArticle.Query() { Id = id });

        return result is not null
            ? TypedResults.Ok(result)
            : TypedResults.NotFound();
    }

    private async Task<IResult> CreateArticle([FromBody] CreateArticleRequest request, IMediator mediator)
    {
        var command = new CreateArticle.Command { Request = request };
        var result = await mediator.Send(command);

        return TypedResults.Created($"/api/articles/{result}", result);
    }

    private async Task<IResult> UpdateArticle([FromRoute] int id, [FromBody] UpdateArticleRequest request, IMediator mediator)
    {
        var command = new UpdateArticle.Command { Id = id, Request = request };
        var result = await mediator.Send(command);

        return TypedResults.Ok();
    }

    private async Task<IResult> DeleteArticle([FromRoute] int id, IMediator mediator)
    {
        var command = new DeleteArticle.Command { Id = id };
        var result = await mediator.Send(command);

        return TypedResults.NoContent();
    }
}
