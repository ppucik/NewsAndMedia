using ASPNETCoreWebAPI.Entities.Interfaces;

namespace ASPNETCoreWebAPI.Entities;

/// <summary>
/// Stránka
/// </summary>
public class Site : IAuditableEntity
{
    public long Id { get; set; } // Primary key

    public DateTimeOffset CreatedAt { get; set; }

    public virtual ICollection<Article> Articles { get; set; } = null!;

    // Implement interface AuditableEntity
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}
