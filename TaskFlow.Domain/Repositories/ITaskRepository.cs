using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Domain.Repositories
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetAllTasksAsync();
        System.Threading.Tasks.Task<Entities.Task> GetTaskByIdAsync(Guid taskId);
        System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetTasksByUserIdAsync(Guid userId);
        System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetTasksByStatusAsync(StatusEnum status);
        System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetTasksByPriorityAsync(PriorityEnum priority);
        System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetTasksByTimeLimitAsync(DateTime timeLimit);
        System.Threading.Tasks.Task<IEnumerable<Entities.Task>> GetTasksByTitleAsync(string title);
        System.Threading.Tasks.Task AddTaskAsync(Entities.Task task);
        System.Threading.Tasks.Task UpdateTaskAsync(Entities.Task task);
        System.Threading.Tasks.Task DeleteTaskAsync(Guid taskId);
        System.Threading.Tasks.Task<bool> TaskExistsAsync(Guid taskId);
    }
}
