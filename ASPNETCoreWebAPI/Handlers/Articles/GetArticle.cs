using ASPNETCoreWebAPI.Contracts;
using ASPNETCoreWebAPI.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreWebAPI.Handlers.Articles;

public class GetArticle
{
    public class Query : IRequest<ArticleResponse?>
    {
        public required long Id { get; set; }
    }

    public class QueryHandler : IRequestHandler<Query, ArticleResponse?>
    {
        private readonly AppDbContext _dataContext;

        public QueryHandler(AppDbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<ArticleResponse?> Handle(Query query, CancellationToken cancellationToken)
        {
            var response = await _dataContext.Articles
                .AsNoTracking()
                .Select(article => new ArticleResponse
                {
                    Id = article.Id,
                    Title = article.Title,
                    Authors = String.Join(", ", article.Authors.Select(a => a.Name)) ?? "(anonymous)",
                    SiteId = article.Site.Id,
                    Created = article.Created,
                    Modified = article.Modified
                })
                .FirstOrDefaultAsync(article => article.Id == query.Id);

            return response;
        }
    }
}
