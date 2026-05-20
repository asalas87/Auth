using Application.Documents.Renovation.DTOs;
using Application.Documents.Renovation.GetById;
using Domain.Documents.Entities;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.Renovation.GetById;
public class GetRenovationByIdQueryHandler(IRenovationRepository renovationRepository) : IRequestHandler<GetRenovationByIdQuery, ErrorOr<RenovationResponseDTO>>
{
    private readonly IRenovationRepository _renovationRepository = renovationRepository ?? throw new ArgumentNullException(nameof(renovationRepository));

    public async Task<ErrorOr<RenovationResponseDTO>> Handle(GetRenovationByIdQuery request, CancellationToken cancellationToken)
    {
        var item = await _renovationRepository.GetByIdAsync(new DocumentFileId(request.Id));
        if (item is null)
        {
            return Error.NotFound("Renovation not found.");
        }
        return new RenovationResponseDTO
        {
            Id = item.Id.Value,
            Name = item.Name,
            Description = item.Description,
            ExpirationDate = item.ExpirationDate,
            RelativePath = item.RelativePath,
            UploadDate = item.UploadDate,
            UploadedBy = item.UploadedBy.Name,
            AssignedTo = item.AssignedTo?.Name ?? string.Empty,
            AssignedToId = item.AssignedTo?.Id.Value,
            Validity = item.Validity,
            RenovationNumber = item.RenovationNumber,
            CertificateNumber = item.CertificateNumber,
            EmployerFullName = item.EmployerFullName,
            StandardCode = item.StandardCode
        };
    }
}
