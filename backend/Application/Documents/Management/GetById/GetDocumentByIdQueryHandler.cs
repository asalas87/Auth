using Application.Common.Dtos;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Documents.Management.GetById;

public class GetDocumentByIdQueryHandler(IDocumentFileRepository documentFileRepository, IWebHostEnvironment env) : IRequestHandler<GetDocumentByIdQuery, ErrorOr<FileDownloadDTO>>
{
    private readonly IDocumentFileRepository _documentFileRepository = documentFileRepository ?? throw new ArgumentNullException(nameof(documentFileRepository));
    private readonly IWebHostEnvironment _env = env ?? throw new ArgumentNullException(nameof(env));

    public async Task<ErrorOr<FileDownloadDTO>> Handle(GetDocumentByIdQuery query, CancellationToken cancellationToken)
    {
        DocumentFile? document = await _documentFileRepository.GetById(new DocumentFileId(query.Id));

        if (document is null)
            return Error.NotFound(description: "Documento no encontrado");

        var path = DocumentFile.BuildRelativePath(_env.WebRootPath, document.RelativePath ?? string.Empty);

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return Error.NotFound(description: "Archivo no encontrado en el servidor");

        var content = await File.ReadAllBytesAsync(path, cancellationToken);

        return new FileDownloadDTO
        {
            Content = content,
            FileName = document.Name ?? "document.pdf",
            ContentType = "application/pdf"
        };
    }
}
