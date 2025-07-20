using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Enums;

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

        public Task(Guid id, string title, string description, PriorityEnum priority, StatusEnum status, DateTime createdAt, DateTime? timeLimit, Guid? userId)
            : base(id)
        {
            Title = title;
            Description = description;
            Priority = priority;
            Status = status;
            CreatedAt = createdAt;
            TimeLimit = timeLimit;
            UserId = userId;
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
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            if (title.Length > 100)
                throw new ArgumentException("Title cannot exceed 100 characters.", nameof(title));
            if (title == Title)
                throw new ArgumentException("Title must be different from the current title.", nameof(title));

            Title = title;
        }
        /// <summary>
        /// Update description of the task
        /// </summary>
        /// <param name="description">The description of the current task</param>
        /// <exception cref="ArgumentException">Empty description</exception>
        /// <exception cref="ArgumentException">Rate limit exceded</exception>
        /// <exception cref="ArgumentException">Same description</exception>
        public void UpdateDescription(string description) {
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty.", nameof(description));
            if (description.Length > 500)
                throw new ArgumentException("Description cannot exceed 500 characters.", nameof(description));
            if (description == Description)
                throw new ArgumentException("Description must be different from the current description.", nameof(description));
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
    }
}
