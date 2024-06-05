using ASPNETCoreWebAPI.Contracts;
using ASPNETCoreWebAPI.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreWebAPI.Handlers.Articles;

public class UpdateArticle
{
    public class Command : IRequest<long>
    {
        public required long Id { get; set; }
        public required UpdateArticleRequest Request { get; set; } = new();
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

            var article = await _dataContext.Articles
                .Include(e => e.Authors)
                .SingleAsync(a => a.Id == command.Id, cancellationToken);

            var authors = await _dataContext.Authors
                .Where(a => request.Authors.Contains(a.Id))
                .ToListAsync(cancellationToken);

            article.Title = request.Title;
            article.Authors = authors;
            article.SiteId = request.SiteId;

            await _dataContext.SaveChangesAsync(cancellationToken);

            return article.Id;
        }
    }
}
