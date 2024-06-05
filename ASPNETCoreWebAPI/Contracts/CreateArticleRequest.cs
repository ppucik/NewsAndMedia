namespace ASPNETCoreWebAPI.Contracts;

/// <summary>
/// Zaevidovanie nového článku
/// </summary>
public class CreateArticleRequest
{
    /// <summary>
    /// Názov článku
    /// </summary>
    public string Title { get; set; } = default!;

    /// <summary>
    /// Zoznam autorov
    /// </summary>
    public long[] Authors { get; set; } = new long[0];

    /// <summary>
    /// TD web stránky
    /// </summary>
    public long SiteId { get; set; }
}
