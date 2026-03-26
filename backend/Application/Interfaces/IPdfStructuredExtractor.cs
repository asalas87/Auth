using Application.Common;

namespace Application.Interfaces;

public interface IPdfStructuredExtractor
{
    List<PdfLine> ExtractLines(Stream pdfStream);
}
