using Application.Documents.Renovation.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Documents.Renovation.GetById;
public record GetRenovationByIdQuery(Guid Id) : IRequest<ErrorOr<RenovationResponseDTO>>;
