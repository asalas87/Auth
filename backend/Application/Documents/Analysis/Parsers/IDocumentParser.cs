using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Documents.Analysis.Dtos;

namespace Application.Documents.Analysis.Parsers
{
    public interface IDocumentParser
    {
        ParsedDocumentResultDto Parse(string text);
    }

}
