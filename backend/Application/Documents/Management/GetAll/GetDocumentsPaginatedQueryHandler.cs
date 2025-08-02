using Application.Common.Responses;
using Application.Documents.Management.DTOs;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetAll
{
    public sealed class GetDocumentsPaginatedQueryHandler : IRequestHandler<GetDocumentsPaginatedQuery, ErrorOr<PaginatedResult<DocumentResponseDTO>>>,
                                                           IRequestHandler<GetDocumentsPaginatedByAssignedToQuery, ErrorOr<PaginatedResult<DocumentResponseDTO>>>
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
                Path = d.Path,
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
            var (documents, totalCount) = await _documentRepository.GetPaginatedByAssignedToAsync(request.Page, request.PageSize, request.Filter, request.assignedTo);

            var items = documents.Select(d => new DocumentResponseDTO
            {
                Id = d.Id.Value,
                Description = d.Description,
                ExpirationDate = d.ExpirationDate,
                Name = d.Name,
                Path = d.Path,
                UploadDate = d.UploadDate

            }).ToList();

            return new PaginatedResult<DocumentResponseDTO>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
