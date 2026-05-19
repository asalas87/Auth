using Application.Common.Responses;
using Application.Documents.Renovation.DTOs;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.Renovation.GetAll;
public sealed class GetRenovationsPaginatedQueryHandler(IRenovationRepository renovationRepository) : IRequestHandler<GetRenovationsPaginatedQuery, ErrorOr<PaginatedResult<RenovationResponseDTO>>>
{
    private readonly IRenovationRepository _renovationRepository = renovationRepository ?? throw new ArgumentNullException(nameof(renovationRepository));

    public async Task<ErrorOr<PaginatedResult<RenovationResponseDTO>>> Handle(GetRenovationsPaginatedQuery request, CancellationToken cancellationToken)
    {
        var (itemsList, totalCount) = await _renovationRepository.GetPaginatedByAssignedToAsync(request.Page, request.PageSize, request.Filter, null);

        var items = itemsList.Select(d => new RenovationResponseDTO
        {
            Id = d.Id.Value,
            Name = d.Name,
            UploadDate = d.UploadDate,
            AssignedToId = d.AssignedTo?.Id.Value,
            AssignedTo = d.AssignedTo?.Name ?? string.Empty,
            ExpirationDate = d.ExpirationDate,
            Validity = d.Validity,
            CertificateNumber = d.CertificateNumber,
            RenovationNumber = d.RenovationNumber,
            EmployerFullName = d.EmployerFullName,
            StandardCode = d.StandardCode
        }).ToList();

        return new PaginatedResult<RenovationResponseDTO>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}
