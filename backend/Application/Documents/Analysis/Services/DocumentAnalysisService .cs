using Application.Documents.Analysis.Dtos;
using Application.Documents.Analysis.Factories;
using Application.Interfaces;
using Domain.Enums;
using Domain.Partners.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Analysis.Services
{
    public class DocumentAnalysisService(
        IPdfTextExtractor textExtractor,
        IPdfStructuredExtractor structuredExtractor,
        IDocumentParserFactory parserFactory,
        ICompanyRepository companyRepository) : IDocumentAnalysisService
    {
        private readonly IPdfTextExtractor _textExtractor = textExtractor;
        private readonly IPdfStructuredExtractor _structuredExtractor = structuredExtractor;
        private readonly IDocumentParserFactory _parserFactory = parserFactory;
        private readonly ICompanyRepository _companyRepository = companyRepository;

        public async Task<ParsedDocumentResultDto> AnalyzeAsync(
            IFormFile file,
            DocumentType documentType)
        {
            var parser = _parserFactory.Resolve(documentType);
            using var stream = file.OpenReadStream();
            var text = parser.ExtractText(stream, _textExtractor);
            stream.Position = 0;

            var lines = _structuredExtractor.ExtractLines(stream);
            var parsed = parser.Parse(text, lines);

            Guid? companyId = null;

            if (!string.IsNullOrWhiteSpace(parsed.CompanyName))
            {
                string normalizedName = parsed.CompanyName
                    .Trim()
                    .TrimEnd('.')
                    .ToLower();
                var company = await _companyRepository
                    .FindByNameAsync(normalizedName);

                companyId = company?.Id.Value;
            }

            return parsed with
            {
                CompanyId = companyId,
                CompanyName = parsed.CompanyName,
                CertificateNumber = parsed.CertificateNumber,
                Validity = parsed.Validity,
                EmployeeFullName = parsed.EmployeeFullName,
                StandardCode = parsed.StandardCode,
                RenovationNumber = parsed.RenovationNumber
            };
        }
    }
}
