using Application.Common.Dtos;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Documents.Management.DownloadById;

public class DownloadDocumentByIdQueryHandler(IDocumentFileRepository documentFileRepository, IWebHostEnvironment env, IUnitOfWork unitOfWork) : IRequestHandler<DownloadDocumentByIdQuery, ErrorOr<FileDownloadDTO>>
{
    private readonly IDocumentFileRepository _documentFileRepository = documentFileRepository ?? throw new ArgumentNullException(nameof(documentFileRepository));
    private readonly IWebHostEnvironment _env = env ?? throw new ArgumentNullException(nameof(env));
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<ErrorOr<FileDownloadDTO>> Handle(DownloadDocumentByIdQuery query, CancellationToken cancellationToken)
    {
        DocumentFile? document = await _documentFileRepository.GetByIdAsync(new DocumentFileId(query.Id));

        if (document is null)
            return Error.NotFound(description: "Documento no encontrado");

        var path = DocumentFile.BuildRelativePath(_env.WebRootPath, document.RelativePath ?? string.Empty);

        if (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
            return Error.NotFound(description: "Archivo no encontrado en el servidor");

        var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true);

        document.MarkAsReadIfNeeded();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new FileDownloadDTO
        {
            Content = stream,
            FileName = document.Name ?? "document.pdf",
            ContentType = "application/pdf"
        };
    }
}
