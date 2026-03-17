using System.IO.Compression;
using System.Reflection.Metadata;
using Application.Common.Dtos;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Documents.Management.GetById;

public class GetDocumentByIdsQueryHandler(IDocumentFileRepository documentFileRepository, IWebHostEnvironment env) : IRequestHandler<GetDocumentByIdsQuery, ErrorOr<FileDownloadDTO>>
{
    private readonly IDocumentFileRepository _documentFileRepository = documentFileRepository ?? throw new ArgumentNullException(nameof(documentFileRepository));
    private readonly IWebHostEnvironment _env = env ?? throw new ArgumentNullException(nameof(env));

    public async Task<ErrorOr<FileDownloadDTO>> Handle(GetDocumentByIdsQuery query, CancellationToken cancellationToken)
    {
        List<DocumentFile> documents = await _documentFileRepository.GetListByIdsAsync(query.Ids.ConvertAll(x => new DocumentFileId(x)));

        if (documents.Count == 0)
            return Error.NotFound(description: "Documentos no encontrados");

        using var memoryStream = new MemoryStream();

        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
            foreach (var doc in documents)
            {
                var filePath = DocumentFile.BuildRelativePath(_env.WebRootPath, doc.RelativePath ?? string.Empty);

                if (!File.Exists(filePath))
                    continue;

                var entry = archive.CreateEntry(doc.Name, CompressionLevel.Fastest);

                using var entryStream = entry.Open();
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                await fileStream.CopyToAsync(entryStream);
            }
        }

        return new FileDownloadDTO
        {
            Content = memoryStream.ToArray(),
            FileName = "documents.pdf",
            ContentType = "application/pdf"
        };
    }
}
