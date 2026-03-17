using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetById;
public record GetDocumentByIdsQuery(List<Guid> Ids) : IRequest<ErrorOr<FileDownloadDTO>>;
