using Microsoft.AspNetCore.Http.HttpResults;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.TaskCases
{
    public class GetAllTasksHandler : IRequestHandler<GetAllTasksCommand, IEnumerable<TaskDTO>>
    {
        private readonly ITaskRepository _taskRepository;
        public GetAllTasksHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        public async Task<IEnumerable<TaskDTO>> HandleAsync(GetAllTasksCommand request)
        {
            return await _taskRepository.GetAllTasksAsync()
                .ContinueWith(tasks =>
                {
                    var filteredTasks = tasks.Result.AsEnumerable();
                    if (request.status.HasValue)
                        filteredTasks = filteredTasks.Where(t => t.Status == request.status.Value);
                    if (request.priority.HasValue)
                        filteredTasks = filteredTasks.Where(t => t.Priority == request.priority.Value);
                    if (request.user != null && user.Id != Guid.Empty)
                        filteredTasks = filteredTasks.Where(t => t.UserId == request.user.Id);
                    if (request.timeLimit.HasValue)
                        filteredTasks = filteredTasks.Where(t => t.TimeLimit == request.timeLimit.Value);
                    if (request.createdAt.HasValue)
                        filteredTasks = filteredTasks.Where(t => t.CreatedAt.Date == request.createdAt.Value.Date);

                    return filteredTasks.Select(t => Mappers.TaskMapper.ToDto(t));
                });
        }
    }
}
