using Application.Documents.Common.DTOs;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetAll;
public record GetExpiringQuery(int batchSize) : IRequest<ErrorOr<List<DocumentExpiringDTO>>>;
