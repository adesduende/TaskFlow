using FluentValidation;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.GroupCases.UpdateGroups
{
    public class UpdateGroupValidator : AbstractValidator<UpdateGroupCommand>
    {
        public UpdateGroupValidator(IUserRepository userRepository, ITaskRepository taskRepository)
        {
            RuleFor(x => x)
                .NotNull().WithMessage("UpdateGroupCommand cannot be null.");
            
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Group ID cannot be empty.")
                .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("Invalid Group ID format.");
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Group name cannot be empty.")
                .MaximumLength(100).WithMessage("Group name cannot exceed 100 characters.");
            
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Group description cannot exceed 500 characters.");
            
            RuleFor(x => x.Users)
                // This rule checks if the users list is null or has a maximum of 100 users.
                .Must(users => users == null || users.Count <= 100)
                .WithMessage("Users list cannot exceed 100 users.")
                .When(x => x.Users != null)
                // This rule checks if each user ID in the users list exists in the repository.
                .ForEach(user =>
                    user.MustAsync(async (id, cancellation) => await userRepository.GetUserByIdAsync(id) is not null)
                        .WithMessage("User does not exist."));
            
            RuleFor(x => x.Tasks)
                // This rule checks if the tasks list is null or has a maximum of 100 tasks.
                .Must(tasks => tasks == null || tasks.Count <= 100)
                .WithMessage("Tasks list cannot exceed 100 tasks.")
                .When(x => x.Tasks != null)
                // This rule checks if each task ID in the tasks list exists in the repository.
                .ForEach(task =>
                    task.MustAsync(async (id, cancellation) => await taskRepository.GetTaskByIdAsync(id) is not null)
                        .WithMessage("Task does not exist."));
        }
    }
}
