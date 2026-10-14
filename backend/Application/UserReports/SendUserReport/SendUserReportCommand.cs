using ErrorOr;
using MediatR;

namespace Application.UserReports.SendUserReport;

public record SendUserReportCommand(
    string Message,
    string? UserEmail,
    string? TraceId,
    string? Url,
    string? UserAgent
) : IRequest<ErrorOr<Success>>;