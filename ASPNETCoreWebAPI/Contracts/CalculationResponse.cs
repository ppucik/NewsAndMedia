using System.Text.Json.Serialization;

namespace ASPNETCoreWebAPI.Contracts;

/// <summary>
/// Výsledok výpočtu
/// </summary>
public class CalculationResponse
{
    /// <summary>
    /// Vložená hodnota Input
    /// </summary>
    [JsonPropertyName("input_value")]
    public decimal InputValue { get; set; }

    /// <summary>
    /// Vypočítaná hodnota zapísaná do global storage
    /// </summary>
    [JsonPropertyName("computed_value")]
    public double ComputedValue { get; set; }

    /// <summary>
    /// Predošlá hodnota v global storage pred výpočtom
    /// </summary>
    [JsonPropertyName("previous_value")]
    public double? PreviousValue { get; set; }

    /// <summary>
    /// Dátum a čas vytvorenia
    /// </summary>
    [JsonIgnore]
    public DateTime Created { get; set; }
}
