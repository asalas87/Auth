using Domain.Enums;
using Domain.Partners.Entities;
using Domain.Primitives;
using Domain.Security.Entities;

namespace Domain.Documents.Entities
{
    public abstract class DocumentFile : AggergateRoot<DocumentFileId>
    {
        public DocumentFile(string name, string path, DateTime uploadDate, DateTime? expirationDate, string description, User uploadedBy, Company? assignedTo, DocumentType documentType)
        {
            Name = name;
            RelativePath = path;
            UploadDate = uploadDate;
            ExpirationDate = expirationDate;
            Description = description;
            UploadedBy = uploadedBy;
            AssignedTo = assignedTo;
            DocumentType = documentType;
        }
        public DocumentFile() { }
        public string Name { get; protected set; } = string.Empty;
        public string Description { get; protected set; } = string.Empty;
        public DateTime UploadDate { get; protected set; }
        public DateTime? ExpirationDate { get; protected set; }
        public User UploadedBy { get; protected set; } = null!;
        public Company? AssignedTo { get; protected set; }
        public string RelativePath { get; protected set; } = string.Empty;
        public DocumentType DocumentType { get; protected set; }
        public void Update(string name, string description, DateTime? expirationDate, Company? assignedTo)
        {
            Description = description;
            ExpirationDate = expirationDate;
            AssignedTo = assignedTo;
        }

        public void DeletePhysicalFile(string webRootPath)
        {
            var fullPath = Path.Combine(webRootPath, RelativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public static string BuildFolderPath(string webRootPath, string subFolder)
        {
            return Path.Combine(webRootPath, subFolder);
        }

        public static string BuildRelativePath(string subFolder, string fileName)
        {
            return Path.Combine(subFolder, fileName);
        }
    }
}
