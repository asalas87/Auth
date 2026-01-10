using Application.Documents.Analysis.Dtos;
using Application.Documents.Analysis.Factories;
using Domain.Enums;
using Domain.Partners.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Analysis.Services
{
    public class DocumentAnalysisService(
        IPdfTextExtractor textExtractor,
        IDocumentParserFactory parserFactory,
        ICompanyRepository companyRepository) : IDocumentAnalysisService
    {
        private readonly IPdfTextExtractor _textExtractor = textExtractor;
        private readonly IDocumentParserFactory _parserFactory = parserFactory;
        private readonly ICompanyRepository _companyRepository = companyRepository;

        public async Task<ParsedDocumentResultDto> AnalyzeAsync(
            IFormFile file,
            DocumentType documentType)
        {
            using var stream = file.OpenReadStream();

            var text = _textExtractor.ExtractText(stream);

            var parser = _parserFactory.Resolve(documentType);

            var parsed = parser.Parse(text);

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
                StandardCode = parsed.StandardCode
            };
        }
    }

}
