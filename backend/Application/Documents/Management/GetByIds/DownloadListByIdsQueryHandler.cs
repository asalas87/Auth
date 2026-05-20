using System.IO.Compression;
using Application.Common.Dtos;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Documents.Management.GetById;

public class DownloadListByIdsQueryHandler(IDocumentFileRepository documentFileRepository, IWebHostEnvironment env) : IRequestHandler<DownloadListByIdsQuery, ErrorOr<FileDownloadDTO>>
{
    private readonly IDocumentFileRepository _documentFileRepository = documentFileRepository ?? throw new ArgumentNullException(nameof(documentFileRepository));
    private readonly IWebHostEnvironment _env = env ?? throw new ArgumentNullException(nameof(env));

    public async Task<ErrorOr<FileDownloadDTO>> Handle(DownloadListByIdsQuery query, CancellationToken cancellationToken)
    {
        List<DocumentFile> documents = await _documentFileRepository.GetListByIdsAsync(query.Ids.ConvertAll(x => new DocumentFileId(x)));

        if (documents.Count == 0)
            return Error.NotFound(description: "Documentos no encontrados");

        var memoryStream = new MemoryStream();

        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            foreach (var doc in documents)
            {
                var filePath = DocumentFile.BuildRelativePath(_env.WebRootPath, doc.RelativePath ?? string.Empty);

                if (!File.Exists(filePath))
                    continue;

                doc.MarkAsReadIfNeeded();

                var entry = archive.CreateEntry(doc.Name, CompressionLevel.Fastest);

                using var entryStream = entry.Open();
                using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                await fileStream.CopyToAsync(entryStream);
            }
        }

        memoryStream.Position = 0;

        return new FileDownloadDTO
        {
            Content = memoryStream,
            FileName = "documents.zip",
            ContentType = "application/zip"
        };
    }
}
