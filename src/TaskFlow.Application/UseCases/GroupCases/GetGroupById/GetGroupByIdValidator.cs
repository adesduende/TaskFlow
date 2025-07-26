using FluentValidation;

namespace TaskFlow.Application.UseCases.GroupCases.GetGroupById
{
    public class GetGroupByIdValidator : AbstractValidator<GetGroupByIdQuery>
    {
        public GetGroupByIdValidator()
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Group ID cannot be empty.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid Group ID format.");
        }
    }
}
