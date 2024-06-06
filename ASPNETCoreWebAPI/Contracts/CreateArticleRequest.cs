using FluentValidation;

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

    public class Validator : AbstractValidator<CreateArticleRequest>
    {
        public Validator()
        {
            RuleFor(c => c.Title)
                .MinimumLength(10)
                .WithMessage("Minimálna dĺžka názvu musí byť 10 znakov")
                .MaximumLength(1000)
                .WithMessage("Maximálna dĺžka názvu môže byť 1000 znakov");

            RuleFor(c => c.Authors)
                .Must(x => x.Any())
               .WithMessage("Musí byť zadaný aspoň jedno ID autora");

            RuleFor(c => c.SiteId)
                .GreaterThan(0)
                .WithMessage("Hodnota ID stránky musí byť väčšia ako nula");
        }
    }
}
