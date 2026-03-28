using Application.Common;
using Application.Documents.Analysis.Dtos;
using Application.Interfaces;

namespace Application.Documents.Analysis.Parsers;
public interface IDocumentParser
{
    string ExtractText(Stream stream, IPdfTextExtractor extractor);
    ParsedDocumentResultDto Parse(string text, List<PdfLine> lines);
}
