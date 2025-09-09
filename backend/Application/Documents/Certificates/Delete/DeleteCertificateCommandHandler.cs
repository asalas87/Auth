using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Documents.Certificates.Delete;

public sealed class DeleteCertificateCommandHandler : IRequestHandler<DeleteCertificateCommand, ErrorOr<Guid>>
{
    private readonly ICertificateRepository _certificateRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;

    public DeleteCertificateCommandHandler(ICertificateRepository certificateRepository, IUnitOfWork unitOfWork, IWebHostEnvironment env)
    {
        _certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _env = env ?? throw new ArgumentNullException(nameof(env));
    }

    public async Task<ErrorOr<Guid>> Handle(DeleteCertificateCommand request, CancellationToken cancellationToken)
    {
        var certificate = await _certificateRepository.GetByIdAsync(new DocumentFileId(request.Id));
        if (certificate is null)
        {
            return Error.NotFound("Certificate.NotFound", "The certificate with the provided Id was not found.");
        }

        certificate.DeletePhysicalFile(_env.WebRootPath);
        _certificateRepository.Delete(certificate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return certificate.Id.Value;
    }
}
