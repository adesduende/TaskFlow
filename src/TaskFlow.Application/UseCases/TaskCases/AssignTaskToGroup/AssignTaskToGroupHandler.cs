using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.TaskCases.AssignTaskToGroup
{
    public class AssignTaskToGroupHandler : IRequestHandler<AssignTaskToGroupCommand,bool>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IGroupRepository _groupRepository;
        public AssignTaskToGroupHandler(ITaskRepository taskRepository, IGroupRepository groupRepository)
        {
            _taskRepository = taskRepository;
            _groupRepository = groupRepository;
        }
        public async Task<bool> HandleAsync(AssignTaskToGroupCommand request)
        {
            var task = await _taskRepository.GetTaskByIdAsync(request.taskId);
            if (task == null)
                return false;

            var group = await _groupRepository.GetByIdAsync(request.groupId);
            if (group == null)
                return false;

            task.AssignGroup(group.Id);
            group.AddTask(task.Id);

            await _taskRepository.UpdateTaskAsync(task);
            await _groupRepository.UpdateAsync(group);

            return true;
        }
    }
}
