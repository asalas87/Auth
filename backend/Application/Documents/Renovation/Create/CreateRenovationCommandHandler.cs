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
using Microsoft.AspNetCore.Hosting;
using SharedKernel.Entities;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Application.Documents.Renovation.Create;
public sealed class CreateRenovationCommandHandler(
    IRenovationRepository renovationRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICompanyRepository companyRepository,
    INotificationRepository notificationRepository,
    IFileStorageService fileStorageService,
    IWebHostEnvironment env) : IRequestHandler<CreateRenovationCommand, ErrorOr<Guid>>
{
    private readonly IRenovationRepository _renovationRepository = renovationRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IWebHostEnvironment _env = env;
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    public async Task<ErrorOr<Guid>> Handle(CreateRenovationCommand request, CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();
        var fileName = $"{documentId}{Path.GetExtension(request.File.FileName)}";
        var uploadDate = DateTime.Now;

        string? relativePath = null;
        try
        {
            if (await _userRepository.GetByIdAsync(new UserId(request.UploadedById)) is not User uploadedUser)
            {
                return Error.NotFound("User.NotFound", "The user with the provided Id was not found.");
            }

            if (await _companyRepository.GetByIdWithUsersAsync(new CompanyId(request.AssignedToId), cancellationToken) is not Company assignedCompany)
            {
                return Error.NotFound("Company.NotFound", "The company with the provided Id was not found.");
            }

            relativePath = await _fileStorageService.SaveAsync(request.File, DocumentType.Renovation, fileName, cancellationToken);

            var renovation = new Domain.Documents.Entities.Renovation(
                request.File.FileName,
                relativePath,
                uploadDate,
                string.Empty,
                uploadedUser,
                assignedCompany,
                request.Validity,
                request.CertificateNumber,
                request.EmployerFullName,
                request.StandardCode,
                request.RenovationNumber
            );

            await _renovationRepository.AddAsync(renovation);

            var notification = new Notification(
                recipientEmail: string.Join(",", assignedCompany.Users.Select(x => x.Email.Value)),
                recipientName: string.Join(",", assignedCompany.Users.Select(x => x.Name)),
                documentId,
                companyId: assignedCompany.Id.Value,
                subject: "Nuevo documento de renovación disponible",
                expirationDate: request.Validity,
                body: request.CertificateNumber,
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

            throw ex ?? new Exception("An error occurred while creating the renovation.");
        }
    }
}
