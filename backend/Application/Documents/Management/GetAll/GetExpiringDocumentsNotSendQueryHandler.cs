using Application.Documents.Common.DTOs;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetAll;
public class GetExpiringDocumentsNotSendQueryHandler : IRequestHandler<GetExpiringDocumentsNotSendQuery, ErrorOr<List<ExpiringDocumentDTO>>>
{
    private readonly IDocumentFileRepository _documentRepository;

    public GetExpiringDocumentsNotSendQueryHandler(IDocumentFileRepository documentFileRepository)
    {
        _documentRepository = documentFileRepository ?? throw new ArgumentNullException(nameof(documentFileRepository));
    }
    public async Task<ErrorOr<List<ExpiringDocumentDTO>>> Handle(GetExpiringDocumentsNotSendQuery request, CancellationToken cancellationToken)
    {
        var documents = await _documentRepository.GetExpiringDocumentsNotSendAsync(request.batchDays);

        return documents.Select(d => new ExpiringDocumentDTO
        {
            DocumentId = d.Id.Value,
            Name = d.Name,
            ExpirationDate = d.ExpirationDate!.Value,
            CompanyId = d.AssignedTo?.Id.Value ?? new Guid(),
            AssignedToEmails = d.AssignedTo?.Users.Select(u => u.Email.Value).ToList() ?? []

        }).ToList();
    }
}
