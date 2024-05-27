using ASPNETCoreWebAPI.Entities.Interfaces;

namespace ASPNETCoreWebAPI.Entities;

/// <summary>
/// Autor
/// </summary>
public class Author : IAuditableEntity
{
    public long Id { get; set; } // Primary key

    public string Name { get; set; } = null!; // Unique index

    public virtual Image Image { get; set; } = null!; // One-To-One

    public virtual ICollection<Article> Articles { get; set; } = null!; // Many-To-Many

    public virtual ICollection<AuthorArticle> AuthorArticles { get; set; } = null!;

    // Implement interface AuditableEntity
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}
