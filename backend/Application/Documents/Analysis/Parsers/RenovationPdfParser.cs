using System.Text.RegularExpressions;
using Application.Common;
using Application.Documents.Analysis.Dtos;
using Application.Interfaces;

namespace Application.Documents.Analysis.Parsers;

public class RenovationPdfParser : IDocumentParser
{
    private static readonly Regex CompanyRegex = new(
        @"Presentado\s+por\s+la\s+empresa:\s*(.+?)(?=Proceso\(s\)\s+de\s+soldadura)",
        RegexOptions.IgnoreCase | RegexOptions.Singleline,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex CertificateNumberRegex = new(
        @"(\d{5}-S-\d{2}-\d{2}-CSI-\d{2}-CSI)",
        RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex FirstNameRegex = new(
        @"Nombre\(s\):\s*(.+?)(?=Identificaci[oó]n)",
        RegexOptions.IgnoreCase | RegexOptions.Singleline,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex LastNameRegex = new(
        @"Apellido\(s\):\s*(.+?)(?=Nombre\(s\))",
        RegexOptions.IgnoreCase | RegexOptions.Singleline,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex ValidityRegex = new(
        @"Per[ií]odo\s+de\s+validez\s+renovado:\s*desde\s+\d{2}/\d{2}/\d{4}\s+hasta\s+(\d{2}/\d{2}/\d{4})",
        RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex StandardCodeRegex = new(
        @"Norma\s*/\s*C[oó]digo\s*/\s*Especificaci[oó]n:\s*(.+?)(?=CERTIFICADO\s+RENOVADO)",
        RegexOptions.IgnoreCase | RegexOptions.Singleline,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex RenovationNumberRegex = new(
        @"Renovación\s+n[úu]mero:\s*(\d+)",
        RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(200));

    public string ExtractText(Stream stream, IPdfTextExtractor extractor) => extractor.ExtractRenovationPageText(stream);

    public ParsedDocumentResultDto Parse(string text, List<PdfLine> lines)
    {
        string? employeeFirstName = Helpers.Match(text, FirstNameRegex);
        string? employeeLastName = Helpers.Match(text, LastNameRegex);
        string employeeFullName = employeeLastName + ", " + employeeFirstName;
        string renovationNumber = Helpers.Match(text, RenovationNumberRegex) ?? "0";
        return new ParsedDocumentResultDto
        {
            CompanyName = Helpers.Match(text, CompanyRegex),
            EmployeeFullName = employeeFullName,
            Validity = Helpers.MatchDate(text, ValidityRegex),
            StandardCode = Helpers.Match(text, StandardCodeRegex),
            CertificateNumber = Helpers.Match(text, CertificateNumberRegex),
            RenovationNumber = Convert.ToInt32(renovationNumber)
        };
    }
}
