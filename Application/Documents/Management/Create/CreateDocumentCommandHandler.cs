using Application.Documents.Management.Create;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using Domain.Secutiry.Entities;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

public sealed class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, ErrorOr<Guid>>
{
    private readonly IDocumentFileRepository _documentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;

    public CreateDocumentCommandHandler(
        IDocumentFileRepository documentRepository,
        IUnitOfWork unitOfWork,
        IWebHostEnvironment env)
    {
        _documentRepository = documentRepository;
        _unitOfWork = unitOfWork;
        _env = env;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();
        var assignedFolder = request.AssignedTo?.ToString() ?? "unassigned";
        var folderPath = Path.Combine(_env.WebRootPath, "documents", assignedFolder);
        var fileName = $"{documentId}{Path.GetExtension(request.File.FileName)}";
        var filePath = Path.Combine(folderPath, fileName);
        var uploadDate = DateTime.Now;

        // Crear carpeta si no existe
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // Guardar archivo
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream, cancellationToken);
        }

        var document = new DocumentFile(
            new DocumentFileId(documentId),
            request.Name,
            Path.Combine("documents", assignedFolder, fileName),
            uploadDate,
            request.ExpirationDate,
            request.Description,
            new(request.UploadedBy),
            request.AssignedTo.HasValue ? new(request.AssignedTo.Value) : null
        );

        await _documentRepository.AddAsync(document);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return document.Id.Value;
    }
}
