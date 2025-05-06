using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Management.Upload
{
    public record UploadDocumentCommand(
        IFormFile File,
        string Name,
        string Description,
        DateTime ExpirationDate,
        Guid RecipientUserId
    ) : IRequest<ErrorOr<Guid>>;
}
