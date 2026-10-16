using ErrorOr;
using MediatR;
using Application.UserReports.SendUserReport;

namespace Application.UserReports;

public sealed class UserReportService(ISender mediator) : IUserReportService
{
    private readonly ISender _mediator = mediator;

    public Task<ErrorOr<Success>> SendAsync(
        SendUserReportCommand command,
        CancellationToken cancellationToken = default)
        => _mediator.Send(command, cancellationToken);
}