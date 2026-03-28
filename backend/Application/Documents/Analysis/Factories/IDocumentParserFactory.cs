using Application.Documents.Analysis.Parsers;
using Domain.Enums;

namespace Application.Documents.Analysis.Factories;
public interface IDocumentParserFactory
{
    IDocumentParser Resolve(DocumentType type);
}
