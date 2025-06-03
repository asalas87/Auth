using Domain.Primitives;
using Domain.Security.Entities;

namespace Domain.Documents.Entities
{
    public sealed class DocumentFile : AggergateRoot<DocumentFileId>
    {
        public DocumentFile(DocumentFileId id, string name, string path, DateTime uploadDate, DateTime? expirationDate, string description, User uploadedBy, User? assignedTo)
        {
            Id = id;
            Name = name;
            Path = path;
            UploadDate = uploadDate;
            ExpirationDate = expirationDate;
            Description = description;
            UploadedBy = uploadedBy;
            AssignedTo = assignedTo;
        }
        public DocumentFile() { }
        public string Name { get; private set; } = string.Empty;
        public string Path { get; private set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Description { get; set; } = string.Empty;
        //public UserId UploadedById { get; private set; } = default!;
        //public UserId? AssignedToId { get; private set; }
        public User UploadedBy { get; set; } = null!;
        public User? AssignedTo { get; set; }
    }
}
