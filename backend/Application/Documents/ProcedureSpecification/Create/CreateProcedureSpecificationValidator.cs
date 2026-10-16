using FluentValidation;

namespace Application.Documents.ProcedureSpecification.Create;

public class CreateProcedureSpecificationValidator : AbstractValidator<CreateProcedureSpecificationCommand>
{
    public CreateProcedureSpecificationValidator()
    {
        RuleFor(command => command.AssignedToId).NotEmpty();
        RuleFor(command => command.ProcedureNumber).NotEmpty().MaximumLength(30);
        RuleFor(command => command.StandardCode).NotEmpty().MaximumLength(100);
        RuleFor(command => command.File).NotNull();
    }
}
