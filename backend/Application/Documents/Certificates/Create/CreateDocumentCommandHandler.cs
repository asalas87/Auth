using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Partners.Entities;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Secutiry.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;

namespace Application.Documents.Certificate.Create;
public sealed class CreateCertificateCommandHandler : IRequestHandler<CreateCertificateCommand, ErrorOr<Guid>>
{
    private readonly ICertificateRepository _certificateRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;

    public CreateCertificateCommandHandler(
        ICertificateRepository documentRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICompanyRepository companyRepository,
        IWebHostEnvironment env)
    {
        _certificateRepository = documentRepository;
        _userRepository = userRepository;
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
        _env = env;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateCertificateCommand request, CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();
        var assignedFolder = request.AssignedTo.ToString();
        var folderPath = Path.Combine(_env.WebRootPath, "certificates", assignedFolder);
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

        if (await _userRepository.GetByIdAsync(new UserId(request.UploadedBy)) is not User uploadedUser)
        {
            return Error.NotFound("User.NotFound", "The user with the provide Id was not found.");
        }

        if (await _companyRepository.GetByIdAsync(new CompanyId(request.AssignedTo)) is not Company assignedComapny)
        {
            return Error.NotFound("Company.NotFound", "The user with the provide Id was not found.");
        }

        var certificate = new Domain.Documents.Entities.Certificate(
            new DocumentFileId(documentId),
            request.Name,
            Path.Combine("documents", assignedFolder, fileName),
            uploadDate,
            request.ValidUntil,
            string.Empty,
            uploadedUser,
            assignedComapny,
            request.ValidFrom,
            request.ValidUntil
        );

        await _certificateRepository.AddAsync(certificate);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return certificate.Id.Value;
    }
}
