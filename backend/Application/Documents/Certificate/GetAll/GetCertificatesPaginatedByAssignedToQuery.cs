using Application.Common.Responses;
using Application.Documents.Certificate.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.GetAll;
public record GetCertificatesPaginatedByAssignedToQuery(int Page, int PageSize, string? Filter, Guid assignedTo) : IRequest<ErrorOr<PaginatedResult<CertificateResponseDTO>>>;
