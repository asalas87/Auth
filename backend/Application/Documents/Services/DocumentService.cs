using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Responses;
using Application.Documents.Certificate.Create;
using Application.Documents.Certificate.DTOs;
using Application.Documents.Certificate.GetAll;
using Application.Documents.Common.DTOs;
using Application.Documents.Management.Create;
using Application.Documents.Management.DTOs;
using Application.Documents.Management.GetAll;
using Application.Documents.Services;
using AutoMapper;
using ErrorOr;
using MediatR;

namespace Infrastructure.Documents.Services;

public class DocumentService : IDocumentService
{
    private readonly IMapper _mapper;
    private readonly ISender _mediator;
    private readonly IAuthenticatedUser _authenticatedUser;
    public DocumentService(ISender mediator,IMapper mapper, IAuthenticatedUser authenticatedUser)
    {
        _mapper = mapper;
        _mediator = mediator;
        _authenticatedUser = authenticatedUser;
    }

    public async Task<ErrorOr<PaginatedResult<DocumentResponseDTO>>> GetDocumentsAsignedToPaginatedAsync(DocumentAssignedDTO documentAssignedDTO)
    {
        var query = _mapper.Map<GetDocumentsPaginatedByAssignedToQuery>(documentAssignedDTO);
        return await _mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<PaginatedResult<DocumentResponseDTO>>>(result);
        });
    }

    public async Task<ErrorOr<PaginatedResult<DocumentResponseDTO>>> GetDocumentsPaginatedAsync(PaginateDTO paginateDTO)
    {
        var query = _mapper.Map<GetDocumentsPaginatedQuery>(paginateDTO);
        return await _mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<PaginatedResult<DocumentResponseDTO>>>(result);
        });
    }

    public async Task<ErrorOr<Guid>> CreateDocumentAsync(DocumentDTO dto)
    {
        var userId = _authenticatedUser.UserId;
        if (userId is null)
            return Error.Failure("Auth", "Usuario no autenticado.");

        var command = _mapper.Map<CreateDocumentCommand>(dto);
        command.UploadedBy = userId.Value;

        return await _mediator.Send(command);
    }

    public async Task<ErrorOr<PaginatedResult<CertificateResponseDTO>>> GetCertificatesPaginatedAsync(PaginateDTO paginateDTO)
    {
        var query = _mapper.Map<GetCertificatesPaginatedQuery>(paginateDTO);
        return await _mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<PaginatedResult<CertificateResponseDTO>>>(result);
        });
    }

    public async Task<ErrorOr<Guid>> CreateCertificateAsync(CertificateDTO dto)
    {
        var query = _mapper.Map<CreateCertificateCommand>(dto);
        return await _mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<Guid>>(result);
        });
    }

    public Task<ErrorOr<Guid>> UpdateCertificateAsync(CertificateEditDTO dto)
    {
        throw new NotImplementedException();
    }

    public Task<ErrorOr<Guid>> DeleteCertificateAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
