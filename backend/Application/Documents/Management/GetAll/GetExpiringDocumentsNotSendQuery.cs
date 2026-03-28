using Application.Documents.Common.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetAll;
public record GetExpiringDocumentsNotSendQuery(int batchDays) : IRequest<ErrorOr<List<ExpiringDocumentDTO>>>;
