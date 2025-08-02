using Application.Common.Responses;
using Application.Documents.Certificate.DTOs;
using Domain.Documents.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.Documents.Certificate.GetAll;
public sealed class GetCertificatesPaginatedQueryHandler : IRequestHandler<GetCertificatesPaginatedQuery, ErrorOr<PaginatedResult<CertificateResponseDTO>>>,
                                                       IRequestHandler<GetCertificatesPaginatedByAssignedToQuery, ErrorOr<PaginatedResult<CertificateResponseDTO>>>
{
    private readonly ICertificateRepository _certificateRepository;

    public GetCertificatesPaginatedQueryHandler(ICertificateRepository certificateRepository)
    {
        _certificateRepository = certificateRepository ?? throw new ArgumentNullException(nameof(certificateRepository));
    }

    public async Task<ErrorOr<PaginatedResult<CertificateResponseDTO>>> Handle(GetCertificatesPaginatedQuery request, CancellationToken cancellationToken)
    {
        var (certificates, totalCount) = await _certificateRepository.GetPaginatedByAssignedToAsync(request.Page, request.PageSize, request.Filter, null);

        var items = certificates.Select(d => new CertificateResponseDTO
        {
            Id = d.Id.Value,
            Description = d.Description,
            ExpirationDate = d.ExpirationDate,
            Name = d.Name,
            Path = d.Path,
            UploadDate = d.UploadDate,
            UploadedBy = d.UploadedBy.Name,
            AssignedTo = d.AssignedTo?.Name ?? string.Empty,
        }).ToList();

        return new PaginatedResult<CertificateResponseDTO>
        {
            Items = items,
            TotalCount = totalCount
        };
    }

    public async Task<ErrorOr<PaginatedResult<CertificateResponseDTO>>> Handle(GetCertificatesPaginatedByAssignedToQuery request, CancellationToken cancellationToken)
    {
        var (certificates, totalCount) = await _certificateRepository.GetPaginatedByAssignedToAsync(request.Page, request.PageSize, request.Filter, request.assignedTo);

        var items = certificates.Select(d => new CertificateResponseDTO
        {
            Id = d.Id.Value,
            Name = d.Name,
            AssignedTo = d.AssignedTo?.Name ?? string.Empty,
            Path = d.Path,
            ValidUntil = d.ValidUntil
        }).ToList();

        return new PaginatedResult<CertificateResponseDTO>
        {
            Items = items,
            TotalCount = totalCount
        };
    }
}
