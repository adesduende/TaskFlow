using FluentValidation;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.TaskCases.CreateTask
{
    public class CreateTaskValidator : AbstractValidator<CreateTaskCommand>
    {
        public CreateTaskValidator(IUserRepository userRepository, IGroupRepository groupRepository)
        {
            RuleFor(x => x.title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");
            RuleFor(x => x.description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
            RuleFor(x => x.timeLimit)
                .GreaterThan(DateTime.UtcNow).WithMessage("Due date must be in the future.");
            RuleFor(x => x.userId)
                .NotEmpty().WithMessage("User ID is required.")
                .MustAsync(async (id,cancellation) => await userRepository.GetUserByIdAsync(id) is not null ? true : false)
                .WithMessage("User does not exist.");
            RuleFor(x => x.groupId)
                .MustAsync(async (id, cancellation) => id == null || await groupRepository.GetByIdAsync(id.Value) is not null)
                .WithMessage("Group does not exist.")
                .When(x => x.groupId.HasValue);
        }
    }
}
