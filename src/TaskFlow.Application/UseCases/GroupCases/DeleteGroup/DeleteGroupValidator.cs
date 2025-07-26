using FluentValidation;

namespace TaskFlow.Application.UseCases.GroupCases.DeleteGroup
{
    public class DeleteGroupValidator : AbstractValidator<DeleteGroupCommand>
    {
        public DeleteGroupValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Group ID cannot be empty.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid Group ID format.");
        }
    }
}
