namespace ASPNETCoreWebAPI.Contracts;

/// <summary>
/// Dotaz na zoznam článku
/// </summary>
public class GetArticlesRequest
{
    /// <summary>
    /// Časť názvu článku (slovo)
    /// </summary>
    public string? FullText { get; set; }

    /// <summary>
    /// Zoznam autorov
    /// </summary>
    public long? Author { get; set; }

    /// <summary>
    /// TD web stránky
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// Číslo stránky
    /// </summary>
    public int PageIndex { get; set; } = 1;

    /// <summary>
    /// Počet záznamov
    /// </summary>
    public int PageCount { get; set; } = 10;
}
