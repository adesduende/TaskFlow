using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.TaskCases.ChangeTaskStatus
{
    public class ChangeTaskStatusHandler : IRequestHandler<ChangeTaskStatusCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;
        public ChangeTaskStatusHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }
        public async Task<bool> HandleAsync(ChangeTaskStatusCommand request)
        {
            if (request.taskId == Guid.Empty)
                throw new ArgumentException("Task ID cannot be empty.", nameof(request.taskId));

            var task = _taskRepository.GetTaskByIdAsync(request.taskId).Result;
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {request.taskId} not found.");

            task.UpdateStatus(request.status);

            await _taskRepository.UpdateTaskAsync(task);

            return true;
        }
    }
}
