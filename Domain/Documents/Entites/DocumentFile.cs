using Domain.Primitives;
using Domain.Secutiry.Entities;

namespace Domain.Documents.Entities
{
    public sealed class DocumentFile : AggergateRoot
    {
        public DocumentFile(DocumentFileId id, string name, string path, DateTime uploadDate, DateTime? expirationDate, string description, UserId uploadedBy, UserId? assignedTo)
        {
            Id = id;
            Name = name;
            Path = path;
            UploadDate = uploadDate;
            ExpirationDate = expirationDate;
            Description = description;
            UploadedById = uploadedBy;
            AssignedToId = assignedTo;
        }
        public DocumentFile()
        {
        }
        public DocumentFileId Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Path { get; private set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public UserId UploadedById { get; set; }
        public UserId? AssignedToId { get; set; }
    }
}
