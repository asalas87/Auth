using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Documents.Analysis.Dtos;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Analysis.Services
{
    public interface IDocumentAnalysisService
    {
        Task<ParsedDocumentResultDto> AnalyzeAsync(
            IFormFile file,
            DocumentType documentType);
    }

}
