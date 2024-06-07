using ASPNETCoreWebAPI.Contracts;
using ASPNETCoreWebAPI.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreWebAPI.Handlers.Articles;

public class GetArticles
{
    public class Query : IRequest<IReadOnlyCollection<ArticleResponse>>
    {
        public required GetArticlesRequest Request { get; set; } = new();
    }

    public class QueryHandler : IRequestHandler<Query, IReadOnlyCollection<ArticleResponse>>
    {
        private readonly AppDbContext _dataContext;
        private readonly IMapper _mapper;

        public QueryHandler(AppDbContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IReadOnlyCollection<ArticleResponse>> Handle(Query query, CancellationToken cancellationToken)
        {
            var request = query.Request;
            var searchText = request.SearchTerm?.ToUpper();
            var sideId = request.SiteId;
            var pageIndex = request.Page ?? 1;
            var pageSize = request.PageSize ?? 10;

            var articles = await _dataContext.Articles
                .AsNoTracking()
                .Include(a => a.Authors)
                .Where(a => string.IsNullOrEmpty(searchText) || (
                    a.Title.ToUpper().Contains(searchText) ||
                    a.Authors.Any(a => a.Name.ToUpper().Contains(searchText))))
                .Where(a => !sideId.HasValue || a.SiteId == sideId)
                .OrderBy(article => article.Title)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IReadOnlyCollection<ArticleResponse>>(articles);
        }
    }
}
