using FluentValidation;
using Application.UserReports.SendUserReport;

namespace Application.UserReports.SendUserReport;

public class SendUserReportCommandValidator : AbstractValidator<SendUserReportCommand>
{
    public SendUserReportCommandValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(5000);

        RuleFor(x => x.UserEmail)
            .MaximumLength(255)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.UserEmail));

        RuleFor(x => x.TraceId)
            .MaximumLength(100)
            .When(x => !string.IsNullOrWhiteSpace(x.TraceId));

        RuleFor(x => x.Url)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.Url));

        RuleFor(x => x.UserAgent)
            .MaximumLength(500)
            .When(x => !string.IsNullOrWhiteSpace(x.UserAgent));
    }
}