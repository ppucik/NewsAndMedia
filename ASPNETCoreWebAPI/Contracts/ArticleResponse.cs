namespace ASPNETCoreWebAPI.Contracts;

/// <summary>
/// Publikovaný článok
/// </summary>
public class ArticleResponse
{
    public long Id { get; set; }

    /// <summary>
    /// Názov článku
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// Zoznam autorov
    /// </summary>
    public string Authors { get; set; } = default!;

    /// <summary>
    /// TD web stránky
    /// </summary>
    public long SiteId { get; set; }

    /// <summary>
    /// Dátum vytvorenia
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    /// Dátum úpravy
    /// </summary>
    public DateTime? Modified { get; set; }
}
