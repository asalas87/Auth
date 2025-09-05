using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetById;
public record GetDocumentByIdQuery(Guid Id) : IRequest<ErrorOr<FileDownloadDTO>>;
