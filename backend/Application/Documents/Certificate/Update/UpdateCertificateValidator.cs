using FluentValidation;

namespace Application.Documents.Certificate.Update;
public class UpdateCertificateValidator : AbstractValidator<UpdateCertificateCommand>
{
    public UpdateCertificateValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Certificate ID is required.")
            .NotNull().WithMessage("Certificate ID cannot be null.");
        RuleFor(x => x.Validity).NotNull().WithMessage("Date vality cannot be null.");
        RuleFor(r => r.StandardCode).NotNull().WithMessage("Standard or code cannot be null.");
    }
}
