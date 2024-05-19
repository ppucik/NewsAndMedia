namespace ASPNETCoreWebAPI.Entities;

/// <summary>
/// Článok
/// </summary>
//[EntityTypeConfiguration(typeof(ArticleConfiguration))]
public class Article
{
    //[Key]
    public long Id { get; set; } // Primary key

    public string Title { get; set; } = null!; // Index

    public virtual ICollection<Author> Authors { get; set; } = null!; // Many-To-Many

    public virtual ICollection<AuthorArticle> AuthorArticles { get; set; } = null!;

    public long SiteId { get; set; }
    public virtual Site Site { get; set; } = null!; // One-To-Many
}
