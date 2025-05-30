using Application.Common.Responses;
using Application.Documents.Management.DTOs;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetAll
{
    public sealed class GetDocumentsPaginatedQueryHandler : IRequestHandler<GetDocumentsPaginatedQuery, ErrorOr<PaginatedResult<DocumentFileDTO>>>,
                                                           IRequestHandler<GetDocumentsPaginatedByAssignedToQuery, ErrorOr<PaginatedResult<DocumentFileDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDocumentFileRepository _documentRepository;

        public GetDocumentsPaginatedQueryHandler(IDocumentFileRepository documentRepository, IUnitOfWork unitOfWork)
        {
            _documentRepository = documentRepository ?? throw new ArgumentNullException(nameof(documentRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<PaginatedResult<DocumentFileDTO>>> Handle(GetDocumentsPaginatedQuery request, CancellationToken cancellationToken)
        {
            var (documents, totalCount) = await _documentRepository.GetPaginatedByAssignedToAsync(request.Page, request.PageSize, request.Filter, null);

            var items = documents.Select(d => new DocumentFileDTO
            {
                Id = d.Id.Value,
                Description = d.Description,
                ExpirationDate = d.ExpirationDate,
                Name = d.Name,
                Path = d.Path,
                UploadDate = d.UploadDate,
                UploadedBy = d.UploadedBy.Name,
                AssignedTo = d.AssignedTo?.Name
            }).ToList();

            return new PaginatedResult<DocumentFileDTO>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<ErrorOr<PaginatedResult<DocumentFileDTO>>> Handle(GetDocumentsPaginatedByAssignedToQuery request, CancellationToken cancellationToken)
        {
            var (documents, totalCount) = await _documentRepository.GetPaginatedByAssignedToAsync(request.Page, request.PageSize, request.Filter, request.assignedTo);

            var items = documents.Select(d => new DocumentFileDTO
            {
                Id = d.Id.Value,
                Description = d.Description,
                ExpirationDate = d.ExpirationDate,
                Name = d.Name,
                Path = d.Path,
                UploadDate = d.UploadDate

            }).ToList();

            return new PaginatedResult<DocumentFileDTO>
            {
                Items = items,
                TotalCount = totalCount
            };
        }
    }
}
