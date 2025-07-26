using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;
using TaskFlow.Domain.Exceptions;

namespace TaskFlow.Domain.Entities
{
    public class Task : Entity
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public PriorityEnum Priority { get; private set; }
        public StatusEnum Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? TimeLimit { get; private set; }
        public Guid? UserId { get; private set; }
        public Guid? GroupId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Task"/> class with a unique identifier, title, description, priority, status, creation date, time limit, and user ID.
        /// </summary>
        /// <param name="id"> The unique identifier for the task.</param>
        /// <param name="title"> The title of the task.</param>
        /// <param name="description"> The description of the task.</param>
        /// <param name="priority"> The priority of the task.</param>
        /// <param name="status"> The status of the task.</param>
        /// <param name="createdAt"> The date and time when the task was created.</param>
        /// <param name="timeLimit"> The time limit for the task, if any.</param>
        /// <param name="userId"> The ID of the user assigned to the task, if any.</param>
        /// <param name="groupId"> The ID of the group assigned to the task, if any.</param>
        public Task(Guid id, string title, string description, PriorityEnum priority, StatusEnum status, DateTime createdAt, DateTime? timeLimit, Guid userId, Guid? groupId)
            : base(id)
        {
            Title = title;
            Description = description;
            Priority = priority;
            Status = status;
            CreatedAt = createdAt;
            TimeLimit = timeLimit;
            UserId = userId;
            GroupId = groupId;
        }
        /// <summary>
        /// Update the task title
        /// </summary>
        /// <param name="title">The title of the task</param>
        /// <exception cref="ArgumentException">Empty title</exception>
        /// <exception cref="ArgumentException">Exceded limit rate</exception>
        /// <exception cref="ArgumentException">Same title</exception>
        public void UpdateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new TaskException.EmptyTitleException();
            if (title.Length > 100)
                throw new TaskException.TitleExceedLimitException(100);
            if (title == Description)
                throw new TaskException.TitleException(title);

            Title = title;
        }
        /// <summary>
        /// Update description of the task
        /// </summary>
        /// <param name="description">The description of the current task</param>
        /// <exception cref="ArgumentException">Empty description</exception>
        /// <exception cref="ArgumentException">Rate limit exceded</exception>
        /// <exception cref="ArgumentException">Same description</exception>
        public void UpdateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                throw new TaskException.EmptyDescriptionException();
            if (description.Length > 500)
                throw new TaskException.DescriptionExceedLimitException(500);
            if (description == Description)
                throw new TaskException.DescriptionException(description);
            Description = description;
        }
        /// <summary>
        /// Update task priority
        /// </summary>
        /// <param name="priority"></param>
        /// <exception cref="ArgumentException">Invalid priority</exception>
        public void UpdatePriority(PriorityEnum priority)
        {
            if (!Enum.IsDefined(typeof(PriorityEnum), priority))
                throw new ArgumentException("Invalid priority value.", nameof(priority));
            Priority = priority;
        }
        /// <summary>
        /// Updatate task status
        /// </summary>
        /// <param name="status"> The status of the task</param>
        /// <exception cref="ArgumentException"> Invalid status</exception>
        public void UpdateStatus(StatusEnum status)
        {
            if (!Enum.IsDefined(typeof(StatusEnum), status))
                throw new ArgumentException("Invalid status value.", nameof(status));
            Status = status;
        }
        /// <summary>
        /// Update the user assigned to the task
        /// </summary>
        /// <param name="timeLimit"> The time limit for the task</param>
        /// <exception cref="ArgumentException"> Time limit cannot be in the past</exception>
        public void UpdateTimeLimit(DateTime? timeLimit)
        {
            if (timeLimit.HasValue && timeLimit < CreatedAt)
                throw new ArgumentException("Time limit cannot be in the past.", nameof(timeLimit));
            TimeLimit = timeLimit;
        }
        /// <summary>
        /// Assign a user to the task
        /// </summary>
        /// <param name="userId"> The ID of the user to assign</param>
        /// <exception cref="ArgumentException"> User ID cannot be empty</exception>
        public void AssignUser(Guid userId)
        {
            if (userId == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            UserId = userId;
        }
        /// <summary>
        /// Unassign the user from the task
        /// </summary>
        /// <param name="groupId"> The ID of the group to assign</param>
        /// <exception cref="ArgumentException"> Group ID cannot be empty</exception>
        public void AssignGroup(Guid groupId)
        {
            if (groupId == Guid.Empty)
                throw new ArgumentException("Group ID cannot be empty.", nameof(groupId));
            GroupId = groupId;
        }
        /// <summary>
        /// Unassign the user from the task
        /// </summary>
        public void UnassignGroup()
        {
            GroupId = null;
        }
    }
}
