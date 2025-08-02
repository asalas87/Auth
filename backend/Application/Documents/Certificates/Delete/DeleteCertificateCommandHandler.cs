using Domain.Documents.Interfaces;
using Domain.Primitives;
using MediatR;
using ErrorOr;

namespace Application.Documents.Certificates.Delete;

public sealed class DeleteCertificateCommandHandler : IRequestHandler<DeleteCertificateCommand, ErrorOr<Guid>>
{
    private readonly ICertificateRepository _certificateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCertificateCommandHandler(ICertificateRepository certificateRepository, IUnitOfWork unitOfWork)
    {
        _certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<ErrorOr<Guid>> Handle(DeleteCertificateCommand request, CancellationToken cancellationToken)
    {
        var certificate = await _certificateRepository.GetByIdAsync(request.Id);
        if (certificate is null)
        {
            return Error.NotFound("Certificate.NotFound", "The certificate with the provided Id was not found.");
        }
        _certificateRepository.Delete(certificate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return certificate.Id.Value;
    }
}
