using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.Services
{
    public class TaskService
    {
        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository ?? throw new ArgumentNullException(nameof(taskRepository));
        }

        /// <summary>
        /// Create a new task with the specified parameters.
        /// </summary>
        /// <param name="title"> The title of the task.</param>
        /// <param name="description"> The description of the task.</param>
        /// <param name="priority"> The priority of the task.</param>
        /// <param name="timeLimit"> The time limit for the task, if any.</param>
        /// <param name="userId"> The ID of the user assigned to the task, if any.</param>
        /// <param name="status"> The status of the task. Defaults to NotStarted.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<Guid> CreateTaskAsync(
            string title,
            string description,
            Domain.Enums.PriorityEnum priority,
            DateTime? timeLimit,
            Guid? userId,
            Domain.Enums.StatusEnum status = Domain.Enums.StatusEnum.NotStarted
        )
        {
            var task = new Domain.Entities.Task(Guid.NewGuid(), title, description, priority, status, DateTime.UtcNow, timeLimit, userId);
            await _taskRepository.AddTaskAsync(task);

            return task.Id;
        }

        /// <summary>
        /// Update an existing task with the specified parameters.
        /// </summary>
        /// <param name="taskId"> The ID of the task to update.</param>
        /// <param name="userId"> The ID of the user assigned to the task, if any.</param>
        /// <returns>
        /// <exception cref="ArgumentException"> Task ID cannot be empty.</exception>
        /// <exception cref="KeyNotFoundException"> Task not found.</exception>
        public async Task<bool> AssignTaskToUser(
            Guid taskId,
            Guid userId
        )
        {
            if (taskId == Guid.Empty)
                throw new ArgumentException("Task ID cannot be empty.", nameof(taskId));
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));

            var task = _taskRepository.GetTaskByIdAsync(taskId).Result;
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found.");

            task.UserId = userId;

            await _taskRepository.UpdateTaskAsync(task);

            return true;
        }

        /// <summary>
        /// Change the status of an existing task.
        /// </summary>
        /// <param name="taskId"> The ID of the task to update.</param>
        /// <param name="status"> The new status of the task.</param>
        /// <returns> A task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException"> Task ID cannot be empty.</exception>
        /// <exception cref="KeyNotFoundException"> Task not found.</exception>
        public async Task<bool> ChangeTaskStatus(
            Guid taskId,
            Domain.Enums.StatusEnum status
        )
        {
            if (taskId == Guid.Empty)
                throw new ArgumentException("Task ID cannot be empty.", nameof(taskId));

            var task = _taskRepository.GetTaskByIdAsync(taskId).Result;
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found.");

            task.Status = status;

            await _taskRepository.UpdateTaskAsync(task);

            return true;
        }

        /// <summary>
        /// Get all tasks with optional filters.
        /// </summary>
        /// <param name="status"> The status of the tasks to filter by, if any.</param>
        /// <param name="priority"> The priority of the tasks to filter by, if any.</param>
        /// <param name="user"> The user assigned to the tasks to filter by, if any.</param>
        /// <param name="timeLimit"> The time limit of the tasks to filter by, if any.</param>
        /// <param name="createdAt"> The creation date of the tasks to filter by, if any.</param>
        /// <returns> A task representing the asynchronous operation, containing a collection of tasks.</returns>
        public async Task<IEnumerable<DTO.TaskDTO>> GetTasks(
            Domain.Enums.StatusEnum? status = null,
            Domain.Enums.PriorityEnum? priority = null,
            User? user = null,
            DateTime? timeLimit = null,
            DateTime? createdAt = null
        )
        {
            return await _taskRepository.GetAllTasksAsync()
                .ContinueWith(tasks =>
                {
                    var filteredTasks = tasks.Result.AsEnumerable();
                    if (status.HasValue)
                        filteredTasks = filteredTasks.Where(t => t.Status == status.Value);
                    if (priority.HasValue)
                        filteredTasks = filteredTasks.Where(t => t.Priority == priority.Value);
                    if (user != null && user.Id != Guid.Empty)
                        filteredTasks = filteredTasks.Where(t => t.UserId == user.Id);
                    if (timeLimit.HasValue)
                        filteredTasks = filteredTasks.Where(t => t.TimeLimit == timeLimit.Value);
                    if (createdAt.HasValue)
                        filteredTasks = filteredTasks.Where(t => t.CreatedAt.Date == createdAt.Value.Date);
                    
                    return filteredTasks.Select(t => Mappers.TaskMapper.ToDto(t));
                });
        }

        /// <summary>
        /// Delete a task by its ID.
        /// </summary>
        /// <param name="taskId"> The ID of the task to delete.</param>
        /// <returns> A task representing the asynchronous operation, returning true if the task was deleted successfully.</returns>
        /// <exception cref="ArgumentException"> Task ID cannot be empty.</exception>
        /// <exception cref="KeyNotFoundException"> Task not found.</exception>
        public async Task<bool> DeleteTaskAsync(
            Guid taskId
        )
        {
            if (taskId == Guid.Empty)
                throw new ArgumentException("Task ID cannot be empty.", nameof(taskId));
            return await _taskRepository.TaskExistsAsync(taskId)
                .ContinueWith(exists =>
                {
                    if (!exists.Result)
                        throw new KeyNotFoundException($"Task with ID {taskId} not found.");
                    
                    _taskRepository.DeleteTaskAsync(taskId).Wait();

                    return true;
                });
        }
    }
}
