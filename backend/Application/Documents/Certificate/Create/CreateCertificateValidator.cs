using FluentValidation;

namespace Application.Documents.Certificate.Create;
public class CreateCertificateValidator : AbstractValidator<CreateCertificateCommand>
{
    public CreateCertificateValidator()
    {
        RuleFor(r => r.Validity)
            .NotEmpty()
            .LessThanOrEqualTo(r => r.Validity)
            .WithMessage("Valid From date must be before or equal to Valid Until date.");
        RuleFor(r => r.AssignedToId)
            .NotNull().WithMessage("AssignedToId cannot be null.");
        RuleFor(r => r.StandardCode)
            .NotEmpty().WithMessage("StandardCode cannot be null.");
        RuleFor(r => r.File)
            .NotNull()
            .WithMessage("File cannot be null.");
    }
}
