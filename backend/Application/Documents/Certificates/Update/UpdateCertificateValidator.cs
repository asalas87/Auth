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
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Certificate name is required.")
            .MaximumLength(100).WithMessage("Certificate name cannot exceed 100 characters.");
        RuleFor(x => x.ValidFrom).Null()
            .LessThanOrEqualTo(x => x.ValidUntil)
            .WithMessage("Valid From date must be before or equal to Valid Until date.");
        RuleFor(r => r.ValidUntil)
            .NotEmpty();
    }
}
