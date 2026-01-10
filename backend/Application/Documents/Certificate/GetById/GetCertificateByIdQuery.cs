using Application.Documents.Certificate.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.GetById;
public record GetCertificateByIdQuery(Guid Id) : IRequest<ErrorOr<CertificateResponseDTO>>;
