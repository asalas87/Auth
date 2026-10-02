using FluentValidation;

namespace Application.Documents.Renovation.Create;

public class CreateRenovationValidator : AbstractValidator<CreateRenovationCommand>
{
    public CreateRenovationValidator()
    {
        RuleFor(command => command.Validity).NotEmpty();
        RuleFor(command => command.AssignedToId).NotEmpty();
        RuleFor(command => command.CertificateNumber).NotEmpty().MaximumLength(30);
        RuleFor(command => command.EmployerFullName).NotEmpty().MaximumLength(100);
        RuleFor(command => command.StandardCode).NotEmpty().MaximumLength(100);
        RuleFor(command => command.RenovationNumber).InclusiveBetween(1, 999);
        RuleFor(command => command.File).NotNull();
    }
}