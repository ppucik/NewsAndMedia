using ASPNETCoreWebAPI.Repositories;
using MediatR;

namespace ASPNETCoreWebAPI.Handlers.Articles;

public class DeleteArticle
{
    public class Command : IRequest<Unit>
    {
        public required long Id { get; set; }
    }

    public class CommandHandler : IRequestHandler<Command, Unit>
    {
        private readonly AppDbContext _dataContext;

        public CommandHandler(AppDbContext dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
        }

        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var article = await _dataContext.Articles.FindAsync(command.Id);

            if (article != null)
            {
                _dataContext.Articles.Remove(article);
                await _dataContext.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}
