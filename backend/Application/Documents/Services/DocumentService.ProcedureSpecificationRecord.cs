using Application.Common.Dtos;
using Application.Common.Responses;
using Application.Documents.ProcedureSpecificationRecord.Create;
using Application.Documents.ProcedureSpecificationRecord.Delete;
using Application.Documents.ProcedureSpecificationRecord.Dtos;
using Application.Documents.ProcedureSpecificationRecord.GetAll;
using Application.Documents.ProcedureSpecificationRecord.GetById;
using Application.Documents.ProcedureSpecificationRecord.Update;
using ErrorOr;

namespace Application.Documents.Services;

public partial class DocumentService
{
    public async Task<ErrorOr<PaginatedResult<ProcedureSpecificationRecordResponseDTO>>> GetProcedureSpecificationRecordsPaginatedAsync(PaginateDTO paginateDTO)
    {
        return await mediator.Send(mapper.Map<GetProcedureSpecificationRecordsPaginatedQuery>(paginateDTO));
    }

    public async Task<ErrorOr<ProcedureSpecificationRecordResponseDTO>> GetProcedureSpecificationRecordByIdAsync(Guid id)
    {
        return await mediator.Send(new GetProcedureSpecificationRecordByIdQuery(id));
    }

    public async Task<ErrorOr<Guid>> CreateProcedureSpecificationRecordAsync(ProcedureSpecificationRecordDTO dto)
    {
        if (authenticatedUser.UserId is not Guid userId)
        {
            return Error.Failure("Auth", "Usuario no autenticado.");
        }

        var command = mapper.Map<CreateProcedureSpecificationRecordCommand>(dto);
        command.UploadedById = userId;
        return await mediator.Send(command);
    }

    public async Task<ErrorOr<Guid>> UpdateProcedureSpecificationRecordAsync(ProcedureSpecificationRecordEditDTO dto)
    {
        return await mediator.Send(mapper.Map<UpdateProcedureSpecificationRecordCommand>(dto));
    }

    public async Task<ErrorOr<Guid>> DeleteProcedureSpecificationRecordAsync(Guid id)
    {
        return await mediator.Send(new DeleteProcedureSpecificationRecordCommand(id));
    }
}