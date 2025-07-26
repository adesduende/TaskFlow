using FluentValidation;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.GroupCases.CreateGroup
{
    public class CreateGroupValidator : AbstractValidator<CreateGroupCommand>
    {
        public CreateGroupValidator(IUserRepository userRepository, ITaskRepository taskRepository)
        {
            RuleFor(x => x.name)
                // This rule checks if the group name is not empty and has a maximum length of 100 characters.
                .NotEmpty().WithMessage("Group name cannot be empty.")
                .MaximumLength(100).WithMessage("Group name cannot exceed 100 characters.");
            RuleFor(x => x.description)
                // This rule checks if the description is not empty and has a maximum length of 500 characters.
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
            RuleFor(x => x.users)
                // This rule checks if the users list is null or has a maximum of 100 users.
                .Must(users => users == null || users.Count <= 100)
                .WithMessage("Users list cannot exceed 100 users.")
                .When(x => x.users != null)
                // This rule checks if each user ID in the users list exists in the repository.
                .ForEach(user =>
                    user.MustAsync(async (id, cancellation) => await userRepository.GetUserByIdAsync(id) is not null)
                        .WithMessage("User does not exist."));
            RuleFor(x => x.tasks)
                // This rule checks if the tasks list is null or has a maximum of 100 tasks.
                .Must(tasks => tasks == null || tasks.Count <= 100)
                .WithMessage("Tasks list cannot exceed 100 tasks.")
                .When(x => x.tasks != null)
                // This rule checks if each task ID in the tasks list exists in the repository.
                .ForEach(task =>
                    task.MustAsync(async (id, cancellation) => await taskRepository.GetTaskByIdAsync(id) is not null)
                        .WithMessage("Task does not exist."));
            RuleFor(x => x.ownerId)
                // This rule checks if the owner ID is not empty and exists in the user repository.
                .NotEmpty().WithMessage("Owner ID cannot be empty.")
                .MustAsync(async (id, cancellation) => await userRepository.GetUserByIdAsync(id) is not null).WithMessage("Owner does not exist.");
        }
    }
}
