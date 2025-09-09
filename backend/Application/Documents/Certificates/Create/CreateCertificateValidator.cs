using Application.Documents.Certificate.Create;
using FluentValidation;

namespace Application.Documents.Certificates.Create;
public class CreateCertificateValidator : AbstractValidator<CreateCertificateCommand>
{
    public CreateCertificateValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty()
            .MaximumLength(50);
        RuleFor(r => r.ValidFrom)
            .NotEmpty()
            .LessThanOrEqualTo(r => r.ValidUntil)
            .WithMessage("Valid From date must be before or equal to Valid Until date.");
        RuleFor(r => r.ValidUntil)
            .NotEmpty();
        RuleFor(r => r.File)
            .NotNull()
            .WithMessage("File cannot be null.");
    }
}
