using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Infrastructure.Repositories.InMemory
{
    public class TaskInMemoryRepository : ITaskRepository
    {
        private readonly List<Domain.Entities.Task> _tasks;

        public TaskInMemoryRepository()
        {
            _tasks = new List<Domain.Entities.Task>();
        }
        public System.Threading.Tasks.Task AddTaskAsync(Domain.Entities.Task task)
        {
            _tasks.Add(task);

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public System.Threading.Tasks.Task DeleteTaskAsync(Guid taskId)
        {
            _tasks.RemoveAll(t => t.Id == taskId);

            return System.Threading.Tasks.Task.CompletedTask;
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetAllTasksAsync()
        {
            return System.Threading.Tasks.Task.FromResult<IEnumerable<Domain.Entities.Task>>(_tasks);
        }

        public Task<Domain.Entities.Task?> GetTaskByIdAsync(Guid taskId)
        {
            return System.Threading.Tasks.Task.FromResult(_tasks.FirstOrDefault(t => t.Id == taskId));
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByPriorityAsync(PriorityEnum priority)
        {
            return System.Threading.Tasks.Task.FromResult<IEnumerable<Domain.Entities.Task>>(
                _tasks.Where(t => t.Priority == priority));
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByStatusAsync(StatusEnum status)
        {
            return System.Threading.Tasks.Task.FromResult<IEnumerable<Domain.Entities.Task>>(
                _tasks.Where(t => t.Status == status));
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByTimeLimitAsync(DateTime timeLimit)
        {
            return System.Threading.Tasks.Task.FromResult<IEnumerable<Domain.Entities.Task>>(
                _tasks.Where(t => t.TimeLimit.HasValue && t.TimeLimit.Value.Date == timeLimit.Date));
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByTitleAsync(string title)
        {
            return System.Threading.Tasks.Task.FromResult<IEnumerable<Domain.Entities.Task>>(
                _tasks.Where(t => t.Title.Contains(title, StringComparison.OrdinalIgnoreCase)));
        }

        public Task<IEnumerable<Domain.Entities.Task>> GetTasksByUserIdAsync(Guid userId)
        {
            return System.Threading.Tasks.Task.FromResult<IEnumerable<Domain.Entities.Task>>(
                _tasks.Where(t => t.UserId.HasValue && t.UserId.Value == userId));
        }

        public Task<bool> TaskExistsAsync(Guid taskId)
        {
            return System.Threading.Tasks.Task.FromResult(_tasks.Any(t => t.Id == taskId));
        }

        public System.Threading.Tasks.Task UpdateTaskAsync(Domain.Entities.Task task)
        {
            return System.Threading.Tasks.Task.Run(
                () =>
                {
                    var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
                    if (existingTask != null)
                    {
                        existingTask.UpdateTitle(task.Title);
                        existingTask.UpdateDescription(task.Description);
                        existingTask.UpdateStatus(task.Status);
                        existingTask.UpdatePriority(task.Priority);
                        existingTask.UpdateTimeLimit(task.TimeLimit);
                    }
                });
        }
    }
}
