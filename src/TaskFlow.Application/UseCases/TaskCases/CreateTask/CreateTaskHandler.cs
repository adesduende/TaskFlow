using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.TaskCases.CreateTask
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, Guid>
    {
        private readonly ITaskRepository _taskRepository;
        public CreateTaskHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }
        public async Task<Guid> HandleAsync(CreateTaskCommand request)
        {
            var task = new Domain.Entities.Task(
                Guid.NewGuid(), 
                request.title,
                request.description,
                request.priority,
                request.status, 
                DateTime.UtcNow,
                request.timeLimit,
                request.userId
            );
            await _taskRepository.AddTaskAsync(task);

            return task.Id;
        }
    }
}
