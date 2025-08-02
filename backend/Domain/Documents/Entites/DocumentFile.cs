using Domain.Enums;
using Domain.Partners.Entities;
using Domain.Primitives;
using Domain.Security.Entities;

namespace Domain.Documents.Entities
{
    public abstract class DocumentFile : AggergateRoot<DocumentFileId>
    {
        public DocumentFile(DocumentFileId id, string name, string path, DateTime uploadDate, DateTime? expirationDate, string description, User uploadedBy, Company? assignedTo, DocumentType documentType)
        {
            Id = id;
            Name = name;
            Path = path;
            UploadDate = uploadDate;
            ExpirationDate = expirationDate;
            Description = description;
            UploadedBy = uploadedBy;
            AssignedTo = assignedTo;
            DocumentType = documentType;
        }
        public DocumentFile() { }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public User UploadedBy { get; set; } = null!;
        public Company? AssignedTo { get; set; }
        public string Path { get; private set; } = string.Empty;
        public DocumentType DocumentType { get; private set; }
    }
}
