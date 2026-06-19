using System.Text.RegularExpressions;
using Application.Common;
using Application.Documents.Analysis.Dtos;
using Application.Interfaces;

namespace Application.Documents.Analysis.Parsers;

public class QualificationPdfParser : IDocumentParser
{
    private static readonly Regex CompanyRegex = new(
        @"Empresa:\s*(.+?)(?=En\s*presencia|El\s*d[ií]a|Apellido)",
        RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex CertificateNumberRegex = new(
        @"Certificado\s*N[ºo°]\s*:?\s*(\d{5}-S-\d{2}-\d{2}-CSI)",
        RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex FirstNameRegex = new(
        @"Nombre\(s\):\s*(.+?)(?=Documento de identidad)",
        RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex LastNameRegex = new(
        @"Apellido\(s\):\s*(.+?)(?=Ha realizado una calificación)",
        RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex ValidityRegex = new(
        @"hasta\s+el\s+.*?(\d{2}/\d{2}/\d{4})",
        RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(200));

    private static readonly Regex StandardCodeRegex = new(
        @"Conforme\s+con\s+los\s+requerimientos\s+de\s+la\s+Norma\s*/\s*Código\s*/\s*Especificación\s*\(?\d*\)?:?\s*(.+?)(?=En\s*la\(s\)\s*probeta\(s\))",
        RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(200));

    public string ExtractText(Stream stream, IPdfTextExtractor extractor) => extractor.ExtractFirstPageText(stream);

    public ParsedDocumentResultDto Parse(string text, List<PdfLine> lines)
    {
        string? employeeFirstName = Helpers.Match(text, FirstNameRegex);
        string? employeeLastName = Helpers.Match(text, LastNameRegex);
        string employeeFullName = employeeLastName + ", " + employeeFirstName;
        return new ParsedDocumentResultDto
        {
            CompanyName = Helpers.Match(text, CompanyRegex),
            EmployeeFullName = employeeFullName,
            Validity = Helpers.MatchDate(text, ValidityRegex),
            StandardCode = Helpers.Match(text, StandardCodeRegex),
            CertificateNumber = Helpers.Match(text, CertificateNumberRegex)
        };
    }
}
