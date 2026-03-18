using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Documents.Management.Delete;

public sealed class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, ErrorOr<Guid>>
{
    private readonly IDocumentFileRepository _documentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;

    public DeleteDocumentCommandHandler(IDocumentFileRepository documentRepository, IUnitOfWork unitOfWork, IWebHostEnvironment env)
    {
        _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _env = env ?? throw new ArgumentNullException(nameof(env));
    }

    public async Task<ErrorOr<Guid>> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        var document = await _documentRepository.GetByIdAsync(new DocumentFileId(request.Id));
        if (document is null)
        {
            return Error.NotFound("Document.NotFound", "The document with the provided Id was not found.");
        }

        document.DeletePhysicalFile(_env.WebRootPath);
        _documentRepository.Delete(document);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return document.Id.Value;
    }
}
