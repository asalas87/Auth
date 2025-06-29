﻿using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Interfaces;
using Application.Common.Responses;
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

    public async Task<ErrorOr<PaginatedResult<DocumentFileDTO>>> GetDocumentsAsignedToPaginatedAsync(DocumentAssignedDTO documentAssignedDTO)
    {
        var query = _mapper.Map<GetDocumentsPaginatedByAssignedToQuery>(documentAssignedDTO);
        return await _mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<PaginatedResult<DocumentFileDTO>>>(result);
        });
    }

    public async Task<ErrorOr<PaginatedResult<DocumentFileDTO>>> GetDocumentsPaginatedAsync(PaginateDTO paginateDTO)
    {
        var query = _mapper.Map<GetDocumentsPaginatedQuery>(paginateDTO);
        return await _mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<PaginatedResult<DocumentFileDTO>>>(result);
        });
    }

    public async Task<ErrorOr<Guid>> CreateDocumentAsync(CreateDocumentDTO dto)
    {
        var userId = _authenticatedUser.UserId;
        if (userId is null)
            return Error.Failure("Auth", "Usuario no autenticado.");

        var command = _mapper.Map<CreateDocumentCommand>(dto);
        command.UploadedBy = userId.Value;

        return await _mediator.Send(command);
    }
}
