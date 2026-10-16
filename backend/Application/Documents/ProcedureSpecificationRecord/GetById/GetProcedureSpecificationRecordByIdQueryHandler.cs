using Application.Documents.ProcedureSpecificationRecord.Dtos;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecificationRecord.GetById;

public sealed class GetProcedureSpecificationRecordByIdQueryHandler(IProcedureSpecificationRecordRepository repository)
    : IRequestHandler<GetProcedureSpecificationRecordByIdQuery, ErrorOr<ProcedureSpecificationRecordResponseDTO>>
{
    public async Task<ErrorOr<ProcedureSpecificationRecordResponseDTO>> Handle(GetProcedureSpecificationRecordByIdQuery request, CancellationToken cancellationToken)
    {
        if (await repository.GetByIdAsync(new DocumentFileId(request.Id)) is not Domain.Documents.Entities.ProcedureSpecificationRecord document)
        {
            return Error.NotFound("ProcedureSpecificationRecord.NotFound", "The procedure specification record was not found.");
        }

        return new ProcedureSpecificationRecordResponseDTO
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
        };
    }
}