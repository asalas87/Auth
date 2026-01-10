using Application.Common.Responses;
using Application.Documents.Renovation.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Documents.Renovation.GetAll;
public record GetRenovationsPaginatedQuery(int Page, int PageSize, string? Filter) : IRequest<ErrorOr<PaginatedResult<RenovationResponseDTO>>>;
