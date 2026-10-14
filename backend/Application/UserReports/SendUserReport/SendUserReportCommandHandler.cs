using Application.Common.Interfaces;
using Application.Interfaces;
using ErrorOr;
using MediatR;

namespace Application.UserReports.SendUserReport;

public sealed class SendUserReportCommandHandler(
    IErrorReporter errorReporter,
    IAuthenticatedUser authenticatedUser)
    : IRequestHandler<SendUserReportCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> Handle(
        SendUserReportCommand request,
        CancellationToken cancellationToken)
    {
        await errorReporter.ReportUserReportAsync(
            message: request.Message,
            userEmail: request.UserEmail,
            userId: authenticatedUser.UserId?.ToString(),
            traceId: request.TraceId,
            url: request.Url,
            userAgent: request.UserAgent,
            cancellationToken: cancellationToken);

        return Result.Success;
    }
}