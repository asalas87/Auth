using Application.Documents.Common.DTOs;
using Domain.Security.Entities;
using ErrorOr;
using MediatR;

namespace Application.Documents.Management.GetAll;
public record GetUserDocumentsQuery(UserId UserId) : IRequest<ErrorOr<List<DocumentGridResponseDTO>>>;
