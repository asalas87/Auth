﻿using Application.Common.Extensions;
using Application.Interfaces;
using Application.Security.Common.DTOS;
using Application.Security.Common.Responses;
using Application.Security.Users.Create;
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

    public UserService(ISender mediator, IPasswordHasher passwordHasher, IMapper mapper, IAuthenticationService authenticationService)
    {
        _mapper = mapper;
        _mediator = mediator;
        _authenticationService = authenticationService;
    }

    public async Task<ErrorOr<LoginResponse>> RegisterUserAsync(RegisterDTO registerDto)
    {
        var command = _mapper.Map<CreateUserCommand>(registerDto);
        var result = await _mediator.Send(command);
        if (result.IsError)
            return result.Errors;

        var query = _mapper.Map<GetUserByIdQuery>(result.Value);
        return await _mediator.Send(query).BindAsync(user =>
        {
            var response = new LoginResponse(user.Id, user.Name, user.Email, _authenticationService.GenerateToken(user));
            return Task.FromResult<ErrorOr<LoginResponse>>(response);
        });
    }

    public async Task<ErrorOr<LoginResponse>> LoginUserAsync(LoginDTO loginDTO)
    {
        var query = _mapper.Map<GetUserByEmailQuery>(loginDTO);
        return await _mediator.Send(query).BindAsync(user =>
        {
            var response = new LoginResponse(user.Id, user.Name, user.Email, _authenticationService.GenerateToken(user));
            return Task.FromResult<ErrorOr<LoginResponse>>(response);
        });
    }
}