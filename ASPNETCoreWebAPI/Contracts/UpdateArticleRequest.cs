namespace ASPNETCoreWebAPI.Contracts;

/// <summary>
/// Editácia údajov článku
/// </summary>
public class UpdateArticleRequest
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
