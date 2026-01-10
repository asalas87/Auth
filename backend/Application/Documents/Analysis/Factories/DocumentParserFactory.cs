using Application.Documents.Analysis.Parsers;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Documents.Analysis.Factories
{
    public class DocumentParserFactory(IServiceProvider provider) : IDocumentParserFactory
    {
        private readonly IServiceProvider _provider = provider;

        public IDocumentParser Resolve(DocumentType type) =>
            type switch
            {
                DocumentType.Renovation =>
                    _provider.GetRequiredService<RenovationPdfParser>(),

                DocumentType.Qualification =>
                    _provider.GetRequiredService<QualificationPdfParser>(),

                _ => throw new NotSupportedException()
            };
    }

}
