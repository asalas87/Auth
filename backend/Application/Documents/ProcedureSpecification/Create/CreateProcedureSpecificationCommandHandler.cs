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
using SharedKernel.Entities;
using SharedKernel.Enums;
using SharedKernel.Interfaces;

namespace Application.Documents.ProcedureSpecification.Create;

public sealed class CreateProcedureSpecificationCommandHandler(
    IProcedureSpecificationRepository procedureSpecificationRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICompanyRepository companyRepository,
    INotificationRepository notificationRepository,
    IFileStorageService fileStorageService) : IRequestHandler<CreateProcedureSpecificationCommand, ErrorOr<Guid>>
{
    public async Task<ErrorOr<Guid>> Handle(CreateProcedureSpecificationCommand request, CancellationToken cancellationToken)
    {
        if (request.AssignedToId is not Guid assignedToId)
        {
            return Error.Validation("Company.Required", "An assigned company is required.");
        }

        if (await userRepository.GetByIdAsync(new UserId(request.UploadedById), cancellationToken) is not User uploadedUser)
        {
            return Error.NotFound("User.NotFound", "The user was not found.");
        }

        if (await companyRepository.GetByIdWithUsersAsync(new CompanyId(assignedToId), cancellationToken) is not Company assignedCompany)
        {
            return Error.NotFound("Company.NotFound", "The company was not found.");
        }

        var documentId = Guid.NewGuid();
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        var fileName = $"{timestamp}_{request.ProcedureNumber}.pdf";
        string? relativePath = null;

        try
        {
            relativePath = await fileStorageService.SaveAsync(request.File!, DocumentType.WeldingProcedure, fileName, cancellationToken);

            var document = new Domain.Documents.Entities.ProcedureSpecification(
                new DocumentFileId(documentId),
                request.File.FileName,
                relativePath,
                DateTime.UtcNow,
                null,
                string.Empty,
                uploadedUser,
                assignedCompany,
                request.ProcedureNumber,
                request.StandardCode);

            await procedureSpecificationRepository.AddAsync(document);

            var notification = new Notification(
                recipientEmail: string.Join(",", assignedCompany.Users.Select(user => user.Email.Value)),
                recipientName: string.Join(",", assignedCompany.Users.Select(user => user.Name)),
                documentId,
                companyId: assignedCompany.Id.Value,
                subject: "Nuevo documento disponible",
                body: request.ProcedureNumber,
                type: NotificationType.DocumentUploaded);

            await notificationRepository.AddAsync(notification, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return document.Id.Value;
        }
        catch
        {
            if (relativePath is not null)
            {
                await fileStorageService.DeleteAsync(relativePath);
            }

            throw;
        }
    }
}
