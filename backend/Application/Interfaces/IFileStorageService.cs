using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAsync(
        IFormFile file,
        DocumentType documentType,
        string fileName,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string relativePath);

    //string BuildRelativePath(string module, string fileName);
}
