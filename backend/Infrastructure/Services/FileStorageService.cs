using Application.Common;
using Application.Interfaces;
using Domain.Documents.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public sealed class FileStorageService(
    IWebHostEnvironment env) : IFileStorageService
{
    private readonly IWebHostEnvironment _env = env;

    public async Task<string> SaveAsync(
        IFormFile file,
        DocumentType documentType,
        string fileName,
        CancellationToken cancellationToken = default)
    {
        fileName = Helpers.SanitizeFileName(fileName);

        var folderPath = Path.Combine(
            _env.WebRootPath,
            documentType.ToString());

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream, cancellationToken);

        return DocumentFile.BuildRelativePath(documentType.ToString(), fileName);
    }

    public Task DeleteAsync(string relativePath)
    {
        var fullPath = Path.Combine(
            _env.WebRootPath,
            relativePath);

        if (File.Exists(fullPath))
            File.Delete(fullPath);

        return Task.CompletedTask;
    }
}
