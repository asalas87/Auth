using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public sealed record Cuit
{
    public string Value { get; }

    private Cuit(string value) => Value = value;

    /// <summary> Crea un Cuit desde un string. Retorna null si es nulo/vacío. </summary>
    public static Cuit? Create(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        value = value.Replace("-", "").Trim();

        // Validación opcional: formato de 11 dígitos
        // if (!Regex.IsMatch(value, @"^\d{11}$")) return null;

        // Validación opcional: dígito verificador
        // if (!IsValidCuit(value)) return null;

        return new Cuit(value);
    }

    private static bool IsValidCuit(string cuit)
    {
        int[] multipliers = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
        int total = 0;

        for (int i = 0; i < 10; i++)
        {
            total += int.Parse(cuit[i].ToString()) * multipliers[i];
        }

        int mod11 = 11 - (total % 11);
        int checkDigit = mod11 == 11 ? 0 : mod11 == 10 ? 9 : mod11;

        return checkDigit == int.Parse(cuit[10].ToString());
    }
    public override string ToString() => Value ?? string.Empty;
}
