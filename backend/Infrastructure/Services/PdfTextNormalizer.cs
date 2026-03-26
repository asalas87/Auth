using System.Text;
using System.Text.RegularExpressions;
using Application.Interfaces;

namespace Infrastructure.Services;
public class PdfTextNormalizer : IPdfTextNormalizer
{
    public string Normalize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        text = text.Replace("\u0000", " ");
        text = Regex.Replace(text, @"\s+", " ");

        return text;
    }
}
