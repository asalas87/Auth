using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UglyToad.PdfPig;

namespace Infrastructure.Services
{
    public class PdfPigTextExtractor : IPdfTextExtractor
    {
        public string ExtractText(Stream pdfStream)
        {
            using var pdf = PdfDocument.Open(pdfStream);

            return string.Join("\n",
                pdf.GetPages().Select(p => p.Text));
        }
    }

}
