using Application.Common;
using Application.Interfaces;
using UglyToad.PdfPig;

namespace Infrastructure.Services;

public class PdfPigStructuredExtractor : IPdfStructuredExtractor
{
    public List<PdfLine> ExtractLines(Stream stream)
    {
        using var pdf = PdfDocument.Open(stream);
        var page = pdf.GetPage(1);

        var words = page.GetWords();

        var lines = words
            .GroupBy(w => Math.Round(w.BoundingBox.Bottom, 1))
            .OrderByDescending(g => g.Key)
            .Select(g => new PdfLine
            {
                Y = g.Key,
                Text = string.Join(" ", g.Select(w => w.Text))
            })
            .ToList();

        return lines;
    }
}
