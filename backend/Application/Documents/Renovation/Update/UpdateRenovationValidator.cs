using FluentValidation;

namespace Application.Documents.Renovation.Update;

public class UpdateRenovationValidator : AbstractValidator<UpdateRenovationCommand>
{
    public UpdateRenovationValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.Validity).NotEmpty();
        RuleFor(command => command.AssignedToId).NotEmpty();
        RuleFor(command => command.CertificateNumber).NotEmpty().MaximumLength(30);
        RuleFor(command => command.EmployerFullName).NotEmpty().MaximumLength(100);
        RuleFor(command => command.StandardCode).NotEmpty().MaximumLength(100);
        RuleFor(command => command.RenovationNumber).InclusiveBetween(1, 999);
    }
}