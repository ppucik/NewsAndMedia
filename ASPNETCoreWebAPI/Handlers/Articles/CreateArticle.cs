using ASPNETCoreWebAPI.Contracts;
using ASPNETCoreWebAPI.Entities;
using ASPNETCoreWebAPI.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreWebAPI.Handlers.Articles;

public class CreateArticle
{
    public class Command : IRequest<long>
    {
        public required CreateArticleRequest Request { get; set; } = new();
    }

    public class CommandHandler : IRequestHandler<Command, long>
    {
        private readonly AppDbContext _dataContext;

        public CommandHandler(AppDbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<long> Handle(Command command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var authors = await _dataContext.Authors
                .Where(a => request.Authors.Contains(a.Id))
                .ToListAsync(cancellationToken);

            var article = new Article()
            {
                Title = request.Title,
                Authors = authors,
                SiteId = request.SiteId,
            };

            _dataContext.Articles.Add(article);

            await _dataContext.SaveChangesAsync(cancellationToken);

            return article.Id;
        }
    }
}
