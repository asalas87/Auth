using Application.Documents.ProcedureSpecification.Dtos;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.ProcedureSpecification.GetById;

public sealed class GetProcedureSpecificationByIdQueryHandler(IProcedureSpecificationRepository repository)
    : IRequestHandler<GetProcedureSpecificationByIdQuery, ErrorOr<ProcedureSpecificationResponseDTO>>
{
    public async Task<ErrorOr<ProcedureSpecificationResponseDTO>> Handle(GetProcedureSpecificationByIdQuery request, CancellationToken cancellationToken)
    {
        if (await repository.GetByIdAsync(new DocumentFileId(request.Id)) is not Domain.Documents.Entities.ProcedureSpecification document ||
            document.DocumentType != Domain.Enums.DocumentType.WeldingProcedure)
        {
            return Error.NotFound("ProcedureSpecification.NotFound", "The procedure specification was not found.");
        }

        return new ProcedureSpecificationResponseDTO
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