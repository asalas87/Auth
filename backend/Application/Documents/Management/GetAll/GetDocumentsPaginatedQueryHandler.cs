using Application.Common.Responses;
using Application.Documents.Common.DTOs;
using Application.Documents.Management.DTOs;
using Domain.Documents.Interfaces;
using Domain.Security.Entities;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetAll;

public sealed class GetDocumentsPaginatedQueryHandler : IRequestHandler<GetDocumentsPaginatedQuery, ErrorOr<PaginatedResult<DocumentResponseDTO>>>,
                                                       IRequestHandler<GetDocumentsPaginatedByAssignedToQuery, ErrorOr<PaginatedResult<DocumentResponseDTO>>>,
                                                       IRequestHandler<GetExpiringQuery, ErrorOr<List<DocumentExpiringDTO>>>
{
    private readonly IDocumentFileRepository _documentRepository;

    public GetDocumentsPaginatedQueryHandler(IDocumentFileRepository documentRepository)
    {
        _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
    }

    public async Task<ErrorOr<PaginatedResult<DocumentResponseDTO>>> Handle(GetDocumentsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var (documents, totalCount) = await _documentRepository.GetPaginatedByAssignedToAsync(request.Page, request.PageSize, request.Filter, null);

        var items = documents.Select(d => new DocumentResponseDTO
        {
            Id = d.Id.Value,
            Description = d.Description,
            ExpirationDate = d.ExpirationDate,
            Name = d.Name,
            RelativePath = d.RelativePath,
            UploadDate = d.UploadDate,
            UploadedBy = d.UploadedBy.Name,
            AssignedTo = d.AssignedTo?.Name ?? string.Empty
        }).ToList();

        return new PaginatedResult<DocumentResponseDTO>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<ErrorOr<PaginatedResult<DocumentResponseDTO>>> Handle(GetDocumentsPaginatedByAssignedToQuery request, CancellationToken cancellationToken)
    {
        var (documents, totalCount) = await _documentRepository.GetPaginatedByAssignedToAsync(request.Page, request.PageSize, request.Filter, new UserId(request.AssignedToUserId));

        var items = documents.Select(d => new DocumentResponseDTO
        {
            Id = d.Id.Value,
            Description = d.Description,
            ExpirationDate = d.ExpirationDate,
            Name = d.Name,
            RelativePath = d.RelativePath,
            UploadDate = d.UploadDate

        }).ToList();

        return new PaginatedResult<DocumentResponseDTO>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<ErrorOr<List<DocumentExpiringDTO>>> Handle(GetExpiringQuery request, CancellationToken cancellationToken)
    {
        var documents = await _documentRepository.GetExpiringAsync(request.batchSize);

        return documents.Select(d => new DocumentExpiringDTO
        {
            DocumentId = d.Id.Value,
            Name = d.Name,
            ExpirationDate = d.ExpirationDate.Value,
            AssignedToEmails = d.AssignedTo?.Users.Select(u => u.Email.Value).ToList() ?? new List<string>()

        }).ToList();
    }
}
