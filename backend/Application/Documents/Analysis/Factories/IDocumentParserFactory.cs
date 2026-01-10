using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Documents.Analysis.Parsers;
using Domain.Enums;

namespace Application.Documents.Analysis.Factories
{
    public interface IDocumentParserFactory
    {
        IDocumentParser Resolve(DocumentType type);
    }
}
