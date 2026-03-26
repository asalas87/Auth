using System.Text;
using Application.Interfaces;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;

namespace Infrastructure.Services;
public class ITextExtractor : IPdfTextExtractor
{
    public string ExtractFirstPageText(Stream pdfStream)
    {
        using var reader = new PdfReader(pdfStream);
        using var pdf = new PdfDocument(reader);
        var page = pdf.GetPage(1);
        var text = PdfTextExtractor.GetTextFromPage(page);
        return text;
    }

    public string ExtractRenovationPageText(Stream pdfStream)
    {
        using var reader = new PdfReader(pdfStream);
        using var pdf = new PdfDocument(reader);
        var page = pdf.GetPage(pdf.GetNumberOfPages() - 1);
        var text = PdfTextExtractor.GetTextFromPage(page);
        return text;
    }

    public string ExtractText(Stream pdfStream)
    {
        using var reader = new PdfReader(pdfStream);
        using var pdf = new PdfDocument(reader);

        var sb = new StringBuilder();

        for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
        {
            var page = pdf.GetPage(i);
            var text = PdfTextExtractor.GetTextFromPage(page);
            sb.AppendLine(text);
        }

        return sb.ToString();
    }
}
