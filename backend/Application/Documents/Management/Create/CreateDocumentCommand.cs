using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Management.Create
{
    public record CreateDocumentCommand : IRequest<ErrorOr<Guid>>
    {
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public DateTime? ExpirationDate { get; set; }
        public Guid UploadedBy { get; set; }
        public Guid? AssignedTo { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}