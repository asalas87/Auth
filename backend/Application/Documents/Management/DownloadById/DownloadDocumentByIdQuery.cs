using Application.Common.Dtos;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.DownloadById;
public record DownloadDocumentByIdQuery(Guid Id) : IRequest<ErrorOr<FileDownloadDTO>>;
