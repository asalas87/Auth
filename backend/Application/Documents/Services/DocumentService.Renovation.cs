using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Responses;
using Application.Documents.Renovation.Create;
using Application.Documents.Renovation.Delete;
using Application.Documents.Renovation.Dtos;
using Application.Documents.Renovation.DTOs;
using Application.Documents.Renovation.GetAll;
using Application.Documents.Renovation.Update;
using ErrorOr;

namespace Application.Documents.Services;
public partial class DocumentService
{
    public async Task<ErrorOr<PaginatedResult<RenovationResponseDTO>>> GetRenovationsPaginatedAsync(PaginateDTO paginateDTO)
    {
        var query = mapper.Map<GetRenovationsPaginatedQuery>(paginateDTO);
        return await mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<PaginatedResult<RenovationResponseDTO>>>(result);
        });
    }

    public async Task<ErrorOr<Guid>> CreateRenovationAsync(RenovationDTO dto)
    {
        var userId = authenticatedUser.UserId;
        if (userId is null)
            return Error.Failure("Auth", "Usuario no autenticado.");

        var command = mapper.Map<CreateRenovationCommand>(dto);
        command.UploadedById = userId.Value;

        return await mediator.Send(command);
    }

    public async Task<ErrorOr<Guid>> UpdateRenovationAsync(RenovationEditDTO dto)
    {
        var command = mapper.Map<UpdateRenovationCommand>(dto);
        return await mediator.Send(command);
    }

    public async Task<ErrorOr<Guid>> DeleteRenovationAsync(Guid id)
    {
        var command = new DeleteRenovationCommand(id);
        return await mediator.Send(command);
    }
}
