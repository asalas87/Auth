using FluentValidation;

namespace Application.Documents.ProcedureSpecificationRecord.Create;

public class CreateProcedureSpecificationRecordValidator : AbstractValidator<CreateProcedureSpecificationRecordCommand>
{
    public CreateProcedureSpecificationRecordValidator()
    {
        RuleFor(command => command.AssignedToId).NotEmpty();
        RuleFor(command => command.ProcedureNumber).NotEmpty().MaximumLength(30);
        RuleFor(command => command.StandardCode).NotEmpty().MaximumLength(100);
        RuleFor(command => command.File).NotNull();
    }
}
