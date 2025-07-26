using FluentValidation;

namespace TaskFlow.Application.UseCases.GroupCases.UpdateGroups
{
    public class UpdateGroupValidator : AbstractValidator<UpdateGroupCommand>
    {
        public UpdateGroupValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Group ID cannot be empty.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid Group ID format.");
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Group name cannot be empty.")
                .MaximumLength(100).WithMessage("Group name cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Group description cannot exceed 500 characters.");
        }
    }
}
