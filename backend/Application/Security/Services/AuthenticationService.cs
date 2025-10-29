using System.Security.Claims;
using System.Security.Cryptography;
using Application.Interfaces;
using Application.Security.Common.DTOS;
using Application.Security.Common.Responses;
using Application.Security.Users.Activate;
using Application.Security.Users.Create;
using Application.Security.Users.GetById;
using Application.Security.Users.Validate;
using AutoMapper;
using Domain.Security.Entities;
using ErrorOr;
using MediatR;

namespace Application.Security.Services;

public class AuthenticationService(
    IRefreshTokenService refreshTokenService,
    IMediator mediator,
    IMapper mapper,
    IJwtTokenGenerator jwtTokenGenerator) : IAuthenticationService
{
    private readonly IRefreshTokenService _refreshTokenService = refreshTokenService;
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new("sub", user.Id.Value.ToString()),
            new("name", user.Name!),
            new("email", user.Email.Value),
            new("role", user.Role.Name.ToString())
        };

        return _jwtTokenGenerator.GenerateToken(claims, DateTime.UtcNow.AddMinutes(15));
    }

    public async Task<string> GenerateRefreshTokenAsync(Guid userId)
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var expiresOn = DateTime.UtcNow.AddDays(7);

        await _refreshTokenService.GenerateAsync(token, expiresOn, userId);

        return token;
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

        var accessToken = GenerateAccessToken(userResult.Value);
        var refreshToken = await GenerateRefreshTokenAsync(userDto.Id);

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
        var accessToken = GenerateAccessToken(userResult.Value);
        var refreshToken = await GenerateRefreshTokenAsync(userDto.Id);

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
        var newAccessToken = GenerateAccessToken(userResult.Value);
        var newRefreshToken = await GenerateRefreshTokenAsync(userDto.Id);

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

    public async Task<ErrorOr<LoginResponse>> ActivateUserAsync(ActivateAccountDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Token))
            return Error.Validation("Activation.MissingToken", "Token de activación requerido.");

        if (string.IsNullOrWhiteSpace(dto.Password) || string.IsNullOrWhiteSpace(dto.ConfirmPassword))
            return Error.Validation("Activation.PasswordRequired", "La contraseña y la confirmación son requeridas.");

        if (dto.Password != dto.ConfirmPassword)
            return Error.Validation("Activation.PasswordMismatch", "Las contraseñas no coinciden.");

        var validateResult = await _mediator.Send(new ValidateActivationTokenQuery(dto.Token));
        if (validateResult.IsError)
            return validateResult.Errors;

        var activation = validateResult.Value;
        var userId = activation.UserId;

        var activateCmd = new ActivateUserCommand(userId, dto.Password, dto.Token);
        var activateResult = await _mediator.Send(activateCmd);
        if (activateResult.IsError)
            return activateResult.Errors;

        var userQueryResult = await _mediator.Send(new GetUserByIdQuery(userId));
        if (userQueryResult.IsError)
            return userQueryResult.Errors;

        var user = userQueryResult.Value;

        var accessToken = GenerateAccessToken(user);
        var userDto = _mapper.Map<UserDTO>(user);
        var refreshToken = await GenerateRefreshTokenAsync(userDto.Id);

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
}
