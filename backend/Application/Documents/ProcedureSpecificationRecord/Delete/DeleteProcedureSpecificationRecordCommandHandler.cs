using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Documents.ProcedureSpecificationRecord.Delete;

public sealed class DeleteProcedureSpecificationRecordCommandHandler(
    IProcedureSpecificationRecordRepository repository,
    IUnitOfWork unitOfWork,
    IWebHostEnvironment environment) : IRequestHandler<DeleteProcedureSpecificationRecordCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(DeleteProcedureSpecificationRecordCommand request, CancellationToken cancellationToken)
    {
        if (await repository.GetByIdAsync(new DocumentFileId(request.Id)) is not Domain.Documents.Entities.ProcedureSpecificationRecord document)
        {
            return Error.NotFound("ProcedureSpecificationRecord.NotFound", "The procedure specification record was not found.");
        }

        document.DeletePhysicalFile(environment.WebRootPath);
        repository.Delete(document);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return document.Id.Value;
    }
}