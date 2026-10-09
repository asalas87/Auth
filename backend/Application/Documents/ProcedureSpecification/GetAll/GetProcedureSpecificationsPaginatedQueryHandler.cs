using Application.Common.Responses;
using Application.Documents.ProcedureSpecification.Dtos;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecification.GetAll;

public sealed class GetProcedureSpecificationsPaginatedQueryHandler(IProcedureSpecificationRepository repository)
    : IRequestHandler<GetProcedureSpecificationsPaginatedQuery, ErrorOr<PaginatedResult<ProcedureSpecificationResponseDTO>>>
{
    public async Task<ErrorOr<PaginatedResult<ProcedureSpecificationResponseDTO>>> Handle(GetProcedureSpecificationsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var (documents, totalCount) = await repository.GetPaginatedByAssignedToAsync(request.Page, request.PageSize, request.Filter, null);
        var items = documents.Select(document => new ProcedureSpecificationResponseDTO
        {
            Id = document.Id.Value,
            Name = document.Name,
            Description = document.Description,
            UploadDate = document.UploadDate,
            ExpirationDate = document.ExpirationDate,
            RelativePath = document.RelativePath,
            UploadedBy = document.UploadedBy.Name,
            AssignedToId = document.AssignedTo?.Id.Value,
            AssignedTo = document.AssignedTo?.Name ?? string.Empty,
            ProcedureNumber = document.ProcedureNumber,
            StandardCode = document.StandardCode
        }).ToList();

        return new PaginatedResult<ProcedureSpecificationResponseDTO>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}