using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Responses;
using Application.Security.Common.DTOs;
using Application.Security.Common.DTOS;
using Application.Security.Users.Create;
using Application.Security.Users.Delete;
using Application.Security.Users.Edit;
using Application.Security.Users.GetAll;
using Application.Security.Users.GetById;
using AutoMapper;
using Domain.Security.Entities;
using ErrorOr;
using MediatR;

namespace Application.Security.Services;
public class UserService(ISender mediator, IMapper mapper) : IUserService
{
    private readonly IMapper _mapper = mapper;
    private readonly ISender _mediator = mediator;

    public async Task<ErrorOr<PaginatedResult<UserDTO>>> GetUsersPaginatedAsync(PaginateDTO paginateDTO)
    {
        var query = _mapper.Map<GetUsersPaginatedQuery>(paginateDTO);
        return await _mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<PaginatedResult<UserDTO>>>(result);
        });
    }

    public async Task<ErrorOr<List<UserDTO>>> GetUsersAsync()
    {
        return await _mediator.Send(new GetAllUsersQuery()).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<List<UserDTO>>>(result);
        });
    }

    public async Task<ErrorOr<SuccessResponse>> DeleteUserAsync(Guid userId)
    {
        var command = new DeleteUserCommand(new UserId(userId));
        var result = await _mediator.Send(command);

        if (result.IsError)
            return result.Errors;

        return new SuccessResponse("Usuario eliminado correctamente.", userId);
    }

    public async Task<ErrorOr<SuccessResponse>> EditUserAsync(EditUserRequest dto)
    {
        var command = _mapper.Map<EditUserCommand>(dto);
        var result = await _mediator.Send(command);

        if (result.IsError)
            return result.Errors;

        return new SuccessResponse("Usuario editado correctamente.", result.Value.Value);
    }

    public async Task<ErrorOr<SuccessResponse>> CreateUserAsync(EditUserRequest dto)
    {
        var command = _mapper.Map<CreateUserCommand>(dto);
        var result = await _mediator.Send(command);

        if (result.IsError)
            return result.Errors;

        return new SuccessResponse("Usuario creado correctamente.", result.Value);
    }

    public async Task<ErrorOr<UserEditDTO>> GetUserByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));

        if (result.IsError)
            return result.Errors;

        var userDto = _mapper.Map<UserEditDTO>(result.Value);
        return userDto;
    }
}
