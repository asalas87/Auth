using System.Text.RegularExpressions;
using Application.Common;
using Application.Documents.Analysis.Dtos;

namespace Application.Documents.Analysis.Parsers
{
    public class QualificationPdfParser : IDocumentParser
    {
        private static readonly Regex CompanyRegex =
            new(@"Presentado por la Empresa:\s*(.+?)(?=En presencia del|El día:|Apellido\(s\):)",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(200));

        private static readonly Regex CertificateNumberRegex = new(
            @"Certificado\s*N[ºo°]\s*:\s*(S-[A-Z0-9\-\/]+(?:\s*\(Rev\.\s*\d+\))?)",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(200));

        private static readonly Regex FirstNameRegex =
            new(@"Nombre\(s\):\s*(.+?)(?=Documento de identidad)",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(200));

        private static readonly Regex LastNameRegex =
            new(@"Apellido\(s\):\s*(.+?)(?=Ha realizado una calificación)",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(200));

        private static readonly Regex ValidityRegex =
            new(@"hasta\s+el\s+.*?(\d{2}/\d{2}/\d{4})",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(200));

        private static readonly Regex StandardCode =
            new(
                @"Conforme\s+con\s+los\s+requerimientos\s+de\s+la\s+Norma\s*\/\s*Código\s*\/\s*Especificación\s*\(?\d*\)?:?\s*([A-Z][A-Z0-9\s\-\.]*?(?:Ed\.?\s*\d{4})?)\s*(?=En\s+la|En\s+el|Inspector|$)",
                RegexOptions.IgnoreCase,
                TimeSpan.FromMilliseconds(200));

        public ParsedDocumentResultDto Parse(string text)
        {
            string? employeeFirstName = Helpers.Match(text, FirstNameRegex);
            string? employeeLastName = Helpers.Match(text, LastNameRegex);
            string employeeFullName = employeeLastName + ", " + employeeFirstName;
            return new ParsedDocumentResultDto
            {
                CompanyName = Helpers.Match(text, CompanyRegex),
                EmployeeFullName = employeeFullName,
                Validity = Helpers.MatchDate(text, ValidityRegex),
                StandardCode = Helpers.Match(text, StandardCode),
                CertificateNumber = Helpers.Match(text, CertificateNumberRegex)
            };
        }
    }
}
