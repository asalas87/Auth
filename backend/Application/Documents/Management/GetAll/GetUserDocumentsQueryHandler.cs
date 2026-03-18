using Application.Documents.Common.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Documents.Interfaces;
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

    public async Task<ErrorOr<List<DocumentGridResponseDTO>>> Handle(GetUserDocumentsQuery query, CancellationToken cancellationToken)
    {
        return await _documentFileRepository
            .GetUserDocuments(query.UserId)
            .OrderByDescending(x => x is Domain.Documents.Entities.Certificate
                ? ((Domain.Documents.Entities.Certificate)x).ExpirationDate
                : DateTime.MinValue)
            .ProjectTo<DocumentGridResponseDTO>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
