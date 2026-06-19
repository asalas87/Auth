using Application.Interfaces;
using Domain.Documents.Interfaces;
using Domain.Enums;
using Domain.Partners.Entities;
using Domain.Partners.Interfaces;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Interfaces;
using ErrorOr;
using MediatR;
using SharedKernel.Entities;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Application.Documents.Certificate.Create;
public sealed class CreateCertificateCommandHandler(
    ICertificateRepository documentRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICompanyRepository companyRepository,
    INotificationRepository notificationRepository,
    IFileStorageService fileStorageService) : IRequestHandler<CreateCertificateCommand, ErrorOr<Guid>>
{
    private readonly ICertificateRepository _certificateRepository = documentRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    public async Task<ErrorOr<Guid>> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var fileName = $"{timestamp}_{request.CertificateNumber}.pdf";
        string? relativePath = null;
        var uploadDate = DateTime.UtcNow;

        try
        {

            if (await _userRepository.GetByIdAsync(new UserId(request.UploadedById)) is not User uploadedUser)
            {
                return Error.NotFound("User.NotFound", "The user with the provide Id was not found.");
            }

            if (await _companyRepository.GetByIdWithUsersAsync(new CompanyId(request.AssignedToId), cancellationToken) is not Company assignedCompany)
            {
                return Error.NotFound("Company.NotFound", "The user with the provide Id was not found.");
            }

            relativePath = await _fileStorageService.SaveAsync(request.File, DocumentType.Qualification, fileName, cancellationToken);

            var certificate = new Domain.Documents.Entities.Certificate(
                fileName,
                relativePath,
                uploadDate,
                string.Empty,
                uploadedUser,
                assignedCompany,
                request.Validity,
                request.CertificateNumber,
                request.EmployerFullName,
                request.StandardCode
            );

            await _certificateRepository.AddAsync(certificate);

            var notification = new Notification(
                recipientEmail: string.Join(",", assignedCompany.Users.Select(x => x.Email.Value)),
                recipientName: string.Join(",", assignedCompany.Users.Select(x => x.Name)),
                documentId,
                companyId: assignedCompany.Id.Value,
                subject: "Nuevo documento disponible",
                body: request.CertificateNumber,
                expirationDate: request.Validity,
                type: NotificationType.DocumentUploaded
            );
            await _notificationRepository.AddAsync(notification, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return documentId;

        }
        catch (Exception ex)
        {
            if (relativePath is not null)
            {
                await _fileStorageService.DeleteAsync(relativePath);
            }

            throw ex??new Exception("An error occurred while creating the certificate.");
        }
    }
}
