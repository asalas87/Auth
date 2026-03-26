namespace Application.Interfaces;
public interface IPdfTextExtractor
{
    string ExtractText(Stream pdfStream);
    string ExtractFirstPageText(Stream pdfStream);
    string ExtractRenovationPageText(Stream pdfStream);
}
