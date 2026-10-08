using FluentValidation;

namespace Application.Documents.ProcedureSpecification.Update;

public class UpdateProcedureSpecificationValidator : AbstractValidator<UpdateProcedureSpecificationCommand>
{
    public UpdateProcedureSpecificationValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.AssignedToId).NotEmpty();
        RuleFor(command => command.ProcedureNumber).NotEmpty().MaximumLength(30);
        RuleFor(command => command.StandardCode).NotEmpty().MaximumLength(100);
    }
}
