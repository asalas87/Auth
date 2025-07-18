﻿using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using Domain.Primitives;
using Domain.Security.Entities;
using Domain.Security.Interfaces;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Application.Documents.Management.Create;
public sealed class CreateDocumentCommandHandler : IRequestHandler<CreateDocumentCommand, ErrorOr<Guid>>
{
    private readonly IDocumentFileRepository _documentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _env;

    public CreateDocumentCommandHandler(
        IDocumentFileRepository documentRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IWebHostEnvironment env)
    {
        _documentRepository = documentRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _env = env;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateDocumentCommand request, CancellationToken cancellationToken)
    {
        var documentId = Guid.NewGuid();
        var assignedFolder = request.AssignedTo?.ToString() ?? "unassigned";
        var folderPath = Path.Combine(_env.WebRootPath, "documents", assignedFolder);
        var fileName = $"{documentId}{Path.GetExtension(request.File.FileName)}";
        var filePath = Path.Combine(folderPath, fileName);
        var uploadDate = DateTime.Now;

        // Crear carpeta si no existe
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await request.File.CopyToAsync(stream, cancellationToken);
        }

        if (await _userRepository.GetByIdAsync(new UserId(request.UploadedBy)) is not User uploadedUser)
        {
            return Error.NotFound("User.NotFound", "The user with the provide Id was not found.");
        }

        User? assignedUser = request.AssignedTo.HasValue ?  await _userRepository.GetByIdAsync(new UserId(request.AssignedTo.Value)) : null;

        var document = new DocumentFile(
            new DocumentFileId(documentId),
            request.Name,
            Path.Combine("documents", assignedFolder, fileName),
            uploadDate,
            request.ExpirationDate,
            request.Description,
            uploadedUser,
            assignedUser
        );

        await _documentRepository.AddAsync(document);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return document.Id.Value;
    }
}
