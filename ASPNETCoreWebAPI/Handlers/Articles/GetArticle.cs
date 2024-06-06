using ASPNETCoreWebAPI.Contracts;
using ASPNETCoreWebAPI.Repositories;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public QueryHandler(AppDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ArticleResponse?> Handle(Query query, CancellationToken cancellationToken)
        {
            var article = await _dataContext.Articles
                .AsNoTracking()
                .Include(e => e.Authors)
                .Include(e => e.Site)
                .FirstOrDefaultAsync(article => article.Id == query.Id);

            return _mapper.Map<ArticleResponse>(article);
        }
    }
}
