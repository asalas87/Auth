using FluentValidation;

namespace Application.Documents.ProcedureSpecificationRecord.Update;

public class UpdateProcedureSpecificationRecordValidator : AbstractValidator<UpdateProcedureSpecificationRecordCommand>
{
    public UpdateProcedureSpecificationRecordValidator()
    {
        RuleFor(command => command.Id).NotEmpty();
        RuleFor(command => command.AssignedToId).NotEmpty();
        RuleFor(command => command.ProcedureNumber).NotEmpty().MaximumLength(30);
        RuleFor(command => command.StandardCode).NotEmpty().MaximumLength(100);
    }
}
