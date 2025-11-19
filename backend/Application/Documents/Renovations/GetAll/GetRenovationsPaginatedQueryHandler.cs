using Application.Common.Responses;
using Application.Documents.Renovation.DTOs;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.Renovation.GetAll;
public sealed class GetRenovationsPaginatedQueryHandler : IRequestHandler<GetRenovationsPaginatedQuery, ErrorOr<PaginatedResult<RenovationResponseDTO>>>
{
    private readonly IRenovationRepository _renovationRepository;

    public GetRenovationsPaginatedQueryHandler(IRenovationRepository renovationRepository)
    {
        _renovationRepository = renovationRepository ?? throw new ArgumentNullException(nameof(renovationRepository));
    }

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
            ValidFrom = d.ValidFrom,
            CertificateNumber = d.CertificateNumber,
            EmployerName = d.EmployerName,
            Code = d.Code
        }).ToList();

        return new PaginatedResult<RenovationResponseDTO>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}
