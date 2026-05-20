using Application.Interfaces;
using Domain.Documents.Entities;
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


namespace Application.Documents.Management.Create;

public sealed class CreateDocumentCommandHandler(
    IDocumentFileRepository documentRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICompanyRepository companyRepository,
    IFileStorageService fileStorageService) : IRequestHandler<CreateDocumentCommand, ErrorOr<Guid>>
{
    private readonly IDocumentFileRepository _documentRepository = documentRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    public async Task<ErrorOr<Guid>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();
        //var assignedFolder = request.AssignedTo?.ToString() ?? "unassigned";
        //var folderPath = Path.Combine(_env.WebRootPath, "documents", assignedFolder);
        var fileName = $"{documentId}{Path.GetExtension(request.File.FileName)}";
        //var /*filePath*/ = Path.Combine(folderPath, fileName);
        string? relativePath = null;
        try
        {

            // Crear carpeta si no existe
            //if (!Directory.Exists(folderPath))
            //    Directory.CreateDirectory(folderPath);

            //using (var stream = new FileStream(filePath, FileMode.Create))
            //{
            //    await request.File.CopyToAsync(stream, cancellationToken);
            //}

            if (await _userRepository.GetByIdAsync(new UserId(request.UploadedBy)) is not User uploadedUser)
            {
                return Error.NotFound("User.NotFound", "The user with the provide Id was not found.");
            }

            Company? assignedUser = request.AssignedTo.HasValue ? await _companyRepository.GetByIdReadOnlyAsync(new CompanyId(request.AssignedTo.Value), cancellationToken) : null;

            relativePath = await _fileStorageService.SaveAsync(request.File, DocumentType.General, fileName, cancellationToken);
            var uploadDate = DateTime.Now;

            var document = new GeneralDocument(
                request.Name,
                relativePath,
                uploadDate,
                request.ExpirationDate,
                request.Description,
                uploadedUser,
                assignedUser
            );

            await _documentRepository.AddAsync(document);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return document.Id.Value;
        }
        catch (Exception ex)
        {
            if (relativePath is not null)
            {
                await _fileStorageService.DeleteAsync(relativePath);
            }

            throw ex ?? new Exception("An error occurred while creating the document.");
        }
    }
}
