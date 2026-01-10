using Application.Common.Responses;
using Application.Documents.Certificate.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.GetAll;
public record GetCertificatesPaginatedQuery(int Page, int PageSize, string? Filter) : IRequest<ErrorOr<PaginatedResult<CertificateResponseDTO>>>;
