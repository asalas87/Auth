using Application.Documents.Certificate.Update;
using FluentValidation;

namespace Application.Documents.Certificates.Update;
public class UpdateCertificateValidator : AbstractValidator<UpdateCertificateCommand>
{
    public UpdateCertificateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Certificate ID is required.")
            .NotNull().WithMessage("Certificate ID cannot be null.");
        RuleFor(x => x.ValidFrom).NotNull().WithMessage("Date from cannot be null.")
            .LessThanOrEqualTo(x => x.ValidUntil)
            .WithMessage("Valid From date must be before or equal to Valid Until date.");
        RuleFor(r => r.ValidUntil)
            .NotNull().WithMessage("Date until cannot be null.");
    }
}
