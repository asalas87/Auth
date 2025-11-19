using Application.Common.Dtos;
using Application.Common.Extensions;
using Application.Common.Responses;
using Application.Documents.Certificate.Create;
using Application.Documents.Certificate.Delete;
using Application.Documents.Certificate.Dtos;
using Application.Documents.Certificate.GetAll;
using Application.Documents.Certificate.Update;
using ErrorOr;

namespace Application.Documents.Services;
public partial class DocumentService
{
    public async Task<ErrorOr<PaginatedResult<CertificateResponseDTO>>> GetCertificatesPaginatedAsync(PaginateDTO paginateDTO)
    {
        var query = mapper.Map<GetCertificatesPaginatedQuery>(paginateDTO);
        return await mediator.Send(query).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<PaginatedResult<CertificateResponseDTO>>>(result);
        });
    }

    public async Task<ErrorOr<Guid>> CreateCertificateAsync(CertificateDTO dto)
    {
        var userId = authenticatedUser.UserId;
        if (userId is null)
            return Error.Failure("Auth", "Usuario no autenticado.");

        var command = mapper.Map<CreateCertificateCommand>(dto);
        command.UploadedById = userId.Value;

        return await mediator.Send(command).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<Guid>>(result);
        });
    }

    public async Task<ErrorOr<Guid>> UpdateCertificateAsync(CertificateEditDTO dto)
    {
        var command = mapper.Map<UpdateCertificateCommand>(dto);
        return await mediator.Send(command).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<Guid>>(result);
        });
    }

    public async Task<ErrorOr<Guid>> DeleteCertificateAsync(Guid id)
    {
        var command = new DeleteCertificateCommand(id);

        return await mediator.Send(command).BindAsync(result =>
        {
            return Task.FromResult<ErrorOr<Guid>>(result);
        });
    }
}
