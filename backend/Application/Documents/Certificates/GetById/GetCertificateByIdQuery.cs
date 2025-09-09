using Application.Common.Responses;
using Application.Documents.Certificate.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.GetById;
public record GetCertificateByIdQuery(Guid Id) : IRequest<ErrorOr<CertificateResponseDTO>>;
