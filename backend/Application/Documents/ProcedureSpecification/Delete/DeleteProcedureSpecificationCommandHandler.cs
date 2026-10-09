using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Documents.ProcedureSpecification.Delete;

public sealed class DeleteProcedureSpecificationCommandHandler(
    IProcedureSpecificationRepository procedureSpecificationRepository,
    IUnitOfWork unitOfWork,
    IWebHostEnvironment environment) : IRequestHandler<DeleteProcedureSpecificationCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(DeleteProcedureSpecificationCommand request, CancellationToken cancellationToken)
    {
        if (await procedureSpecificationRepository.GetByIdAsync(new DocumentFileId(request.Id)) is not Domain.Documents.Entities.ProcedureSpecification document ||
            document.DocumentType != Domain.Enums.DocumentType.WeldingProcedure)
        {
            return Error.NotFound("ProcedureSpecification.NotFound", "The procedure specification was not found.");
        }

        document.DeletePhysicalFile(environment.WebRootPath);
        procedureSpecificationRepository.Delete(document);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return document.Id.Value;
    }
}