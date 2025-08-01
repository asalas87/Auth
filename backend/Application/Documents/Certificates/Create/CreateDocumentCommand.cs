using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Documents.Certificate.Create
{
    public record CreateCertificateCommand : IRequest<ErrorOr<Guid>>
    {
        public string Name { get; set; } = default!;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        public Guid UploadedBy { get; set; }
        public Guid AssignedTo { get; set; }
        public IFormFile File { get; set; } = default!;
    }
}
