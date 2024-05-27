using ASPNETCoreWebAPI.Entities.Interfaces;

namespace ASPNETCoreWebAPI.Entities;

/// <summary>
/// Obrázok
/// </summary>
public class Image : IAuditableEntity
{
    public long Id { get; set; } // Primary key

    public string Description { get; set; } = null!;

    public long AuthorId { get; set; } // Foreign key
    public virtual Author Author { get; set; } = null!;

    // Implement interface AuditableEntity
    public DateTime Created { get; set; }
    public DateTime? Modified { get; set; }
}
