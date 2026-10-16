using Application.Common.Dtos;
using Application.Common.Responses;
using Application.Documents.ProcedureSpecification.Create;
using Application.Documents.ProcedureSpecification.Delete;
using Application.Documents.ProcedureSpecification.Dtos;
using Application.Documents.ProcedureSpecification.GetAll;
using Application.Documents.ProcedureSpecification.GetById;
using Application.Documents.ProcedureSpecification.Update;
using ErrorOr;

namespace Application.Documents.Services;

public partial class DocumentService
{
    public async Task<ErrorOr<PaginatedResult<ProcedureSpecificationResponseDTO>>> GetProcedureSpecificationsPaginatedAsync(PaginateDTO paginateDTO)
    {
        var query = mapper.Map<GetProcedureSpecificationsPaginatedQuery>(paginateDTO);
        return await mediator.Send(query);
    }

    public async Task<ErrorOr<ProcedureSpecificationResponseDTO>> GetProcedureSpecificationByIdAsync(Guid id)
    {
        return await mediator.Send(new GetProcedureSpecificationByIdQuery(id));
    }

    public async Task<ErrorOr<Guid>> CreateProcedureSpecificationAsync(ProcedureSpecificationDTO dto)
    {
        if (authenticatedUser.UserId is not Guid userId)
        {
            return Error.Failure("Auth", "Usuario no autenticado.");
        }

        var command = mapper.Map<CreateProcedureSpecificationCommand>(dto);
        command.UploadedById = userId;
        return await mediator.Send(command);
    }

    public async Task<ErrorOr<Guid>> UpdateProcedureSpecificationAsync(ProcedureSpecificationEditDTO dto)
    {
        return await mediator.Send(mapper.Map<UpdateProcedureSpecificationCommand>(dto));
    }

    public async Task<ErrorOr<Guid>> DeleteProcedureSpecificationAsync(Guid id)
    {
        return await mediator.Send(new DeleteProcedureSpecificationCommand(id));
    }
}