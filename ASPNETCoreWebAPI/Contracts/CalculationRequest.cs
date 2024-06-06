using FluentValidation;

namespace ASPNETCoreWebAPI.Contracts;

/// <summary>
/// Zadanie výpočtu
/// </summary>
public class CalculationRequest
{
    /// <summary>
    /// Vstupná hodnota výpočtu
    /// </summary>
    public decimal Input { get; set; }

    public class Validator : AbstractValidator<CalculationRequest>
    {
        public Validator()
        {
            RuleFor(c => c.Input)
                .GreaterThan(0)
                .WithMessage("Hodnota Input musí byť väčšia ako nula");
        }
    }
}
