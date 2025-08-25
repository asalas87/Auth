using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Responses;
using Application.Interfaces;
using Application.Security.Common.DTOs;
using Application.Security.Common.DTOS;
using Application.Security.Common.Responses;
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
public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;
    private readonly IAuthenticationService _authenticationService;
    private readonly IRefreshTokenService _refreshTokenService;

    public UserService(ISender mediator, IPasswordHasher passwordHasher, IMapper mapper, IAuthenticationService authenticationService, IRefreshTokenService refreshTokenService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _authenticationService = authenticationService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<ErrorOr<LoginResponse>> RegisterUserAsync(RegisterDTO registerDTO)
    {
        var command = _mapper.Map<CreateUserCommand>(registerDTO);

        var createResult = await _mediator.Send(command);

        if (createResult.IsError)
            return createResult.Errors;

        var query = new GetUserByIdQuery(createResult.Value);
        var userResult = await _mediator.Send(query);

        if (userResult.IsError)
            return userResult.Errors;

        var userDto = _mapper.Map<UserDTO>(userResult.Value);

        var accessToken = _authenticationService.GenerateAccessToken(userResult.Value);
        var refreshToken = await _authenticationService.GenerateRefreshTokenAsync(userDto.Id);

        return new LoginResponse(
            userDto.Id,
            userDto.Name,
            userDto.Email,
            userDto.Role,
            accessToken,
            refreshToken
        );
    }

    public async Task<ErrorOr<LoginResponse>> LoginUserAsync(LoginDTO loginDTO)
    {
        var query = _mapper.Map<GetUserByEmailQuery>(loginDTO);
        var userResult = await _mediator.Send(query);

        if (userResult.IsError)
            return userResult.Errors;

        var userDto = _mapper.Map<UserDTO>(userResult.Value);
        var accessToken = _authenticationService.GenerateAccessToken(userResult.Value);
        var refreshToken = await _authenticationService.GenerateRefreshTokenAsync(userDto.Id);

        var response = new LoginResponse(
            userDto.Id,
            userDto.Name,
            userDto.Email,
            userDto.Role,
            accessToken,
            refreshToken
        );

        return response;
    }

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

    public async Task<ErrorOr<LoginResponse>> RefreshTokenAsync(string refreshToken)
    {
        var isValid = await _refreshTokenService.ValidateAsync(refreshToken);
        if (!isValid)
        {
            return Error.Unauthorized("Invalid or expired refresh token.");
        }

        var existingToken = await _refreshTokenService.GetByValueAsync(refreshToken);
        if (existingToken is null)
        {
            return Error.Unauthorized("Refresh token not found.");
        }

        var userId = existingToken.UserId;
        var userResult = await _mediator.Send(new GetUserByIdQuery(userId.Value));

        var userDto = _mapper.Map<UserDTO>(userResult.Value);
        await _refreshTokenService.RevokeAsync(refreshToken);
        var newAccessToken = _authenticationService.GenerateAccessToken(userResult.Value);
        var newRefreshToken = await _authenticationService.GenerateRefreshTokenAsync(userDto.Id);
       
        var response = new LoginResponse(
            userDto.Id,
            userDto.Name,
            userDto.Email,
            userDto.Role,
            newAccessToken,
            newRefreshToken
        );

        return response;
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

    public async Task<ErrorOr<UserEditDTO>> GetUserByIdAsync(Guid id)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(id));

        if (result.IsError)
            return result.Errors;

        var userDto = _mapper.Map<UserEditDTO>(result.Value);
        return userDto;
    }
}
