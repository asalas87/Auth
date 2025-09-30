using System.Linq;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Partners.Entities;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Secutiry.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using SharedKernel.Entities;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Application.Documents.Certificate.Create;
public sealed class CreateCertificateCommandHandler : IRequestHandler<CreateCertificateCommand, ErrorOr<Guid>>
{
    private readonly ICertificateRepository _certificateRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;

    public CreateCertificateCommandHandler(
        ICertificateRepository documentRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository,
        INotificationRepository notificationRepository,
        IWebHostEnvironment env)
    {
        _certificateRepository = documentRepository;
        _userRepository = userRepository;
        _companyRepository = companyRepository;
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _env = env;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();
        var folderPath = DocumentFile.BuildFolderPath(_env.WebRootPath, "Certificates");
        var fileName = $"{documentId}{Path.GetExtension(request.File.FileName)}";
        var filePath = Path.Combine(folderPath, fileName);
        var uploadDate = DateTime.Now;

        // Crear carpeta si no existe
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream, cancellationToken);
        }

        if (await _userRepository.GetByIdAsync(new UserId(request.UploadedById)) is not User uploadedUser)
        {
            return Error.NotFound("User.NotFound", "The user with the provide Id was not found.");
        }

        if (await _companyRepository.GetByIdWithUsersAsync(new CompanyId(request.AssignedToId)) is not Company assignedComapny)
        {
            return Error.NotFound("Company.NotFound", "The user with the provide Id was not found.");
        }

        var relativePath = DocumentFile.BuildRelativePath("Certificates", fileName);

        var certificate = new Domain.Documents.Entities.Certificate(
            new DocumentFileId(documentId),
            request.Name,
            relativePath,
            uploadDate,
            request.ValidUntil,
            string.Empty,
            uploadedUser,
            assignedComapny,
            request.ValidFrom,
            request.ValidUntil
        );

        await _certificateRepository.AddAsync(certificate);

        var notification = new Notification(
            recipientEmail: string.Join(",", assignedComapny.Users.Select(x => x.Email.Value)),
            documentId,
            subject: "Nuevo documento disponible",
            body: $"Se ha subido un nuevo certificado el {uploadDate:d}",
            type: NotificationType.DocumentUploaded
        );
        await _notificationRepository.AddAsync(notification, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return documentId;
    }
}
