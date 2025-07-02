using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Responses;
using Application.Interfaces;
using Application.Security.Common.DTOS;
using Application.Security.Common.Responses;
using Application.Security.Users.Create;
using Application.Security.Users.GetAll;
using Application.Security.Users.GetById;
using AutoMapper;
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

        var response = new LoginResponse(
            userDto.Id,
            userDto.Name,
            userDto.Email,
            accessToken,
            refreshToken
        );

        return response;
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
            newAccessToken,
            newRefreshToken
        );

        return response;
    }
}
