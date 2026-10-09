using Application.Common.Responses;
using Application.Documents.ProcedureSpecificationRecord.Dtos;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecificationRecord.GetAll;

public sealed class GetProcedureSpecificationRecordsPaginatedQueryHandler(IProcedureSpecificationRecordRepository repository)
    : IRequestHandler<GetProcedureSpecificationRecordsPaginatedQuery, ErrorOr<PaginatedResult<ProcedureSpecificationRecordResponseDTO>>>
{
    public async Task<ErrorOr<PaginatedResult<ProcedureSpecificationRecordResponseDTO>>> Handle(GetProcedureSpecificationRecordsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var (documents, totalCount) = await repository.GetPaginatedByAssignedToAsync(request.Page, request.PageSize, request.Filter, null);
        var items = documents.Select(document => new ProcedureSpecificationRecordResponseDTO
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

        return new PaginatedResult<ProcedureSpecificationRecordResponseDTO>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}