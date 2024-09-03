using Domain.Secutiry.Entities;
using Domain.ValueObjects;

namespace Application.Security.Common.Responses;

public record LoginResponse(
UserId Id,
string Name,
Email Email,
string Token);