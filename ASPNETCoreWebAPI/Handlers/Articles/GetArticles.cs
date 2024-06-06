﻿using ASPNETCoreWebAPI.Contracts;
using ASPNETCoreWebAPI.Repositories;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreWebAPI.Handlers.Articles;

public class GetArticles
{
    public class Query : IRequest<IReadOnlyCollection<ArticleResponse>>
    {
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
            var articles = await _dataContext.Articles
                .AsNoTracking()
                .Include(e => e.Authors)
                .Include(e => e.Site)
                .OrderBy(article => article.Title)
                .ToListAsync(cancellationToken);

            return _mapper.Map<IReadOnlyCollection<ArticleResponse>>(articles);
        }
    }
}
