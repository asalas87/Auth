using Application.Documents.Common.DTOs;
using AutoMapper;
using Domain.Documents.Interfaces;
using Domain.Enums;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Documents.Management.GetAll;

public class GetUserDocumentsQueryHandler : IRequestHandler<GetUserDocumentsQuery, ErrorOr<List<DocumentGridResponseDTO>>>
{
    private readonly IDocumentFileRepository _documentFileRepository;
    private readonly IMapper _mapper;

    public GetUserDocumentsQueryHandler(IDocumentFileRepository documentFileRepository, IMapper mapper)
    {
        _documentFileRepository = documentFileRepository ?? throw new ArgumentNullException(nameof(documentFileRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
    }

    public async Task<ErrorOr<List<DocumentGridResponseDTO>>> Handle(
    GetUserDocumentsQuery query,
    CancellationToken cancellationToken)
    {
        var documents = await _documentFileRepository
            .GetUserDocuments(query.UserId)
            .OrderByDescending(x => x.ExpirationDate ?? DateTime.MinValue)
            .ToListAsync(cancellationToken);

        var result = documents.Select(d => new DocumentGridResponseDTO
        {
            Id = d.Id.Value,
            Name = d.Name,
            IsRead = d.IsRead,
            Validity = d switch
            {
                Domain.Documents.Entities.Certificate c => c.Validity,
                Domain.Documents.Entities.ProcedureSpecificationRecord pr => pr.UploadDate,
                Domain.Documents.Entities.ProcedureSpecification p => p.UploadDate,
                _ => null
            },
            DocumentNumber = d switch
            {
                Domain.Documents.Entities.Certificate c => c.CertificateNumber,
                Domain.Documents.Entities.ProcedureSpecificationRecord pr => pr.ProcedureNumber,
                Domain.Documents.Entities.ProcedureSpecification p => p.ProcedureNumber,
                _ => string.Empty
            },
            StandardCode = d switch
            {
                Domain.Documents.Entities.Certificate c => c.StandardCode,
                Domain.Documents.Entities.ProcedureSpecificationRecord pr => pr.StandardCode,
                Domain.Documents.Entities.ProcedureSpecification p => p.StandardCode,
                _ => string.Empty
            },
            Type = d.DocumentType switch
            {
                DocumentType.Renovation => "Renovación",
                DocumentType.Qualification => "Certificado",
                DocumentType.ProcedureSpecificationRecord => "Registro de Especificación de Procedimiento",
                DocumentType.WeldingProcedure => "Especificación de Procedimiento",
                _ => "Documento"
            }
        }).ToList();

        return result;
    }
}
