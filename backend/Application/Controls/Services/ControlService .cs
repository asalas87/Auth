using Application.Controls.Dtos;
using Application.Controls.Queries;
using ErrorOr;
using MediatR;

namespace Application.Controls.Services;
public class ControlService : IControlService
{
    private readonly ISender _mediator;

    public ControlService(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task<ErrorOr<List<CompanyLookupDto>>> GetAllCompaniesAsync(CancellationToken cancellationToken)
    {
        return await _mediator.Send(new GetAllCompaniesQuery(), cancellationToken);
    }

    //public async Task<List<UserLookupDto>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    //{
    //    return await _mediator.Send(new GetAllUsersQuery(), cancellationToken);
    //}
}
