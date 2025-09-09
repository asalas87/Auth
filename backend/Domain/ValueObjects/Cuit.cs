using System.Text.RegularExpressions;

namespace Domain.ValueObjects;
public sealed class Cuit : IEquatable<Cuit>
{
    private static readonly Regex CuitRegex = new(@"^\d{11}$");

    public string Value { get; }

    private Cuit(string value)
    {
        Value = value;
    }

    public static Cuit Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("El CUIT no puede estar vacío.");

        value = value.Replace("-", "").Trim();

        if (!CuitRegex.IsMatch(value))
            throw new ArgumentException("Formato de CUIT inválido. Debe tener 11 dígitos.");

        // Opcional: Validación del dígito verificador.
        if (!IsValidCuit(value))
            throw new ArgumentException("CUIT inválido (dígito verificador incorrecto).");

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

    public override bool Equals(object? obj) => Equals(obj as Cuit);

    public bool Equals(Cuit? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;

    public static implicit operator string(Cuit cuit) => cuit.Value;
}
