namespace TaskFlow.Application.Mappers
{
    public static class TaskMapper
    {
        /// <summary>
        /// Maps a task entity to a DTO.
        /// </summary>
        /// <param name="task">The task entity to map.</param>
        /// <returns>A DTO representing the task.</returns>
        public static DTO.TaskDTO ToDto(this Domain.Entities.Task task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            return new DTO.TaskDTO(
                task.Id.ToString(),
                task.Title,
                task.Description,
                task.Priority.ToString(),
                task.Status.ToString(),
                task.CreatedAt,
                task.TimeLimit.HasValue ? task.TimeLimit : null,
                task.UserId.HasValue ? task.UserId.Value.ToString() : string.Empty,
                task.GroupId.HasValue ? task.GroupId.Value.ToString() : string.Empty
            );
        }
    }
}
