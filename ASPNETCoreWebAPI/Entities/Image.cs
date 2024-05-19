namespace ASPNETCoreWebAPI.Entities;

/// <summary>
/// Obrázok
/// </summary>
public class Image
{
    public long Id { get; set; } // Primary key

    public string Description { get; set; } = null!;

    public long AuthorId { get; set; } // Foreign key
    public virtual Author Author { get; set; } = null!;
}
