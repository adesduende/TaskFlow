using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.TaskCases
{
    public class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, bool>
    {
        private readonly ITaskRepository _taskRepository;

        public DeleteTaskHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public async Task<bool> HandleAsync(DeleteTaskCommand request)
        {
            if (request.taskId == Guid.Empty)
                throw new ArgumentException("Task ID cannot be empty.", nameof(request.taskId));
            return await _taskRepository.TaskExistsAsync(request.taskId)
                .ContinueWith(exists =>
                {
                    if (!exists.Result)
                        throw new KeyNotFoundException($"Task with ID {request.taskId} not found.");

                    _taskRepository.DeleteTaskAsync(request.taskId).Wait();

                    return true;
                });
        }
    }
}
