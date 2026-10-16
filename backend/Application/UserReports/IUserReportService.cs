using ErrorOr;
using Application.UserReports.SendUserReport;

namespace Application.UserReports;

public interface IUserReportService
{
    Task<ErrorOr<Success>> SendAsync(
        SendUserReportCommand command,
        CancellationToken cancellationToken = default);
}