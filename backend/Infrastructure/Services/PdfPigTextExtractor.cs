using Application.Interfaces;
using UglyToad.PdfPig;

namespace Infrastructure.Services;
public class PdfPigTextExtractor : IPdfTextExtractor
{
    public string ExtractFirstPageText(Stream pdfStream)
    {
        using var pdf = PdfDocument.Open(pdfStream);
        var firstPage = pdf.GetPage(1);
        return firstPage.Text;
    }

    public string ExtractRenovationPageText(Stream pdfStream)
    {
        using var pdf = PdfDocument.Open(pdfStream);
        var firstRenovationPage = pdf.GetPage(pdf.NumberOfPages - 1);
        return firstRenovationPage.Text;
    }

    public string ExtractText(Stream pdfStream)
    {
        using var pdf = PdfDocument.Open(pdfStream);

        return string.Join("\n",
            pdf.GetPages().Select(p => p.Text));
    }
}
