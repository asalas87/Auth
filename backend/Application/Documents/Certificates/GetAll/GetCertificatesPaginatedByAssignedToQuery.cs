using Application.Common.Responses;
using Application.Documents.Certificate.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.GetAll;
public record GetCertificatesPaginatedByAssignedToQuery(int Page, int PageSize, string? Filter, Guid assignedTo) : IRequest<ErrorOr<PaginatedResult<CertificateResponseDTO>>>;
