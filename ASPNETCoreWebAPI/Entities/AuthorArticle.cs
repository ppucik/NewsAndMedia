namespace ASPNETCoreWebAPI.Entities;

public class AuthorArticle
{
    public long AuthorId { get; set; }
    public virtual Author Author { get; set; } = null!;

    public long ArticleId { get; set; }
    public virtual Article Article { get; set; } = null!;
}
