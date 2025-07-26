using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Mappers;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.TaskCases.GetTaskById
{
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, TaskDTO>
    {
        private readonly ITaskRepository _taskRepository;
        public GetTaskByIdHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }
        public async Task<TaskDTO> HandleAsync(GetTaskByIdQuery request)
        {
            var task = await _taskRepository.GetTaskByIdAsync(request.taskId);
            if (task == null)
            {
                throw new KeyNotFoundException($"Task with ID {request.taskId} not found.");
            }

            return task.ToDto();
        }
    }
}
