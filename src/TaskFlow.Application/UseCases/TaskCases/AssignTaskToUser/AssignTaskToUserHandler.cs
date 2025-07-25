using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.TaskCases.AssignTaskToUser
{
    public class AssignTaskToUserHandler : IRequestHandler<AssignTaskToUserCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        public AssignTaskToUserHandler(ITaskRepository taskRepository) 
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        /// <summary>
        /// Assigns a task to a user.
        /// </summary>
        /// <param name="request"> The command containing the task ID and user ID.</param>
        /// <returns> A task representing the asynchronous operation, with a boolean indicating success.</returns>
        /// <exception cref="ArgumentException"> Task ID or User ID cannot be empty.</exception>
        /// <exception cref="KeyNotFoundException"> Task not found.</exception>
        public async Task<bool> HandleAsync (AssignTaskToUserCommand request)
        {
            if (request.taskId == Guid.Empty)
                throw new ArgumentException("Task ID cannot be empty.", nameof(request.taskId));
            if (request.userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(request.userId));

            var task = _taskRepository.GetTaskByIdAsync(request.taskId).Result;
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {request.taskId} not found.");

            task.AssignUser(request.userId);

            await _taskRepository.UpdateTaskAsync(task);

            return true;
        }
    }
}
