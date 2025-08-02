using Application.Common.Responses;
using Application.Documents.Certificate.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.GetAll;
public record GetCertificatesPaginatedQuery(int Page, int PageSize, string? Filter) : IRequest<ErrorOr<PaginatedResult<CertificateResponseDTO>>>;
