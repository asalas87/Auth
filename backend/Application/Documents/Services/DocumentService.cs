using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Responses;
using Application.Documents.Common.DTOs;
using Application.Documents.Management.Create;
using Application.Documents.Management.Delete;
using Application.Documents.Management.DTOs;
using Application.Documents.Management.GetAll;
using Application.Documents.Management.GetById;
using AutoMapper;
using Domain.Security.Entities;
using ErrorOr;
using MediatR;

namespace Application.Documents.Services;

public partial class DocumentService(ISender mediator, IMapper mapper, IAuthenticatedUser authenticatedUser) : IDocumentService
{
    public async Task<ErrorOr<PaginatedResult<DocumentResponseDTO>>> GetDocumentsAsignedToPaginatedAsync(DocumentAssignedDTO documentAssignedDTO)
    {
        var userId = authenticatedUser.UserId;
        if (userId is null)
            return Error.Failure("Auth", "Usuario no autenticado.");
        documentAssignedDTO.AssignedToUserId = userId.Value;

        var query = mapper.Map<GetDocumentsPaginatedByAssignedToQuery>(documentAssignedDTO);

        return await mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<PaginatedResult<DocumentResponseDTO>>>(result);
        });
    }

    public async Task<ErrorOr<PaginatedResult<DocumentResponseDTO>>> GetDocumentsPaginatedAsync(PaginateDTO paginateDTO)
    {
        var query = mapper.Map<GetDocumentsPaginatedQuery>(paginateDTO);
        return await mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<PaginatedResult<DocumentResponseDTO>>>(result);
        });
    }

    public async Task<ErrorOr<List<DocumentGridResponseDTO>>> GetUserDocumentsAsync()
    {
        if (authenticatedUser.UserId is  null)
            return Error.Failure("Auth", "Usuario no autenticado.");

        var userId = authenticatedUser.UserId;

        var query = new GetUserDocumentsQuery(new UserId(userId.Value));
        return await mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<List<DocumentGridResponseDTO>>>(result);
        });
    }

    public async Task<ErrorOr<Guid>> DeleteDocumentAsync(Guid id)
    {
        var command = new DeleteDocumentCommand(id);

        return await mediator.Send(command).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<Guid>>(result);
        });
    }

    public async Task<ErrorOr<Guid>> CreateDocumentAsync(DocumentDTO dto)
    {
        var userId = authenticatedUser.UserId;
        if (userId is null)
            return Error.Failure("Auth", "Usuario no autenticado.");

        var command = mapper.Map<CreateDocumentCommand>(dto);
        command.UploadedBy = userId.Value;

        return await mediator.Send(command);
    }

    public async Task<ErrorOr<FileDownloadDTO>> DownloadAsync(Guid id)
    {
        var query = new GetDocumentByIdQuery(id);

        return await mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<FileDownloadDTO>>(result);
        });
    }

    public async Task<ErrorOr<FileDownloadDTO>> DownloadMultipleAsync(List<Guid> ids)
    {
        var query = new GetDocumentByIdsQuery(ids);

        return await mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<FileDownloadDTO>>(result);
        });
    }
}
