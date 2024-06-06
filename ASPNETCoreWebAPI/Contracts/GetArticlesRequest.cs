using System.ComponentModel;

namespace ASPNETCoreWebAPI.Contracts;

/// <summary>
/// Dotaz na zoznam článkov
/// </summary>
public class GetArticlesRequest
{
    /// <summary>
    /// Časť názvu článku, autor (slovo)
    /// </summary>
    public string? SearchText { get; set; }

    /// <summary>
    /// TD web stránky
    /// </summary>
    public long? SiteId { get; set; }

    /// <summary>
    /// Číslo stránky (dafault 1)
    /// </summary>
    [DefaultValue(1)]
    public int? Page { get; set; } = 1;

    /// <summary>
    /// Počet záznamov (dafault 10)
    /// </summary>
    [DefaultValue(10)]
    public int? PageSize { get; set; } = 10;
}
