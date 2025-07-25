using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Domain.Aggregates
{
    public class Group : Entity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Guid> Users { get; private set; }
        public List<Guid> Tasks { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class with a unique identifier, name, and description.
        /// </summary>
        /// <param name="id"> The unique identifier for the group.</param>
        /// <param name="name"> The name of the group.</param>
        /// <param name="description"> The description of the group.</param>
        public Group(Guid id, string name, string description, List<Guid> users, List<Guid> tasks)
        : base(id)
        {
            Name = name;
            Description = description;
            Users = users ?? new List<Guid>();
            Tasks = tasks ?? new List<Guid>();
        }

        /// <summary>
        /// Adds a user to the group if they are not already a member.
        /// </summary>
        /// <param name="userId"> The ID of the user to add.</param>
        public void AddUser(Guid userId)
        {
            if (!Users.Contains(userId))
            {
                Users.Add(userId);
            }
        }

        /// <summary>
        /// Removes a user from the group if they are a member.
        /// </summary>
        /// <param name="userId"> The ID of the user to remove.</param>
        public void RemoveUser(Guid userId)
        {
            if (Users.Contains(userId))
            {
                Users.Remove(userId);
            }

        }

        /// <summary>
        /// Adds a task to the group if it is not already assigned.
        /// </summary>
        /// <param name="taskId"> The ID of the task to add.</param>
        public void AddTask(Guid taskId)
        {
            if (!Tasks.Contains(taskId))
            {
                Tasks.Add(taskId);
            }
        }

        /// <summary>
        /// Removes a task from the group if it is assigned.
        /// </summary>
        /// <param name="taskId"> The ID of the task to remove.</param>
        public void RemoveTask(Guid taskId)
        {
            if (Tasks.Contains(taskId))
            {
                Tasks.Remove(taskId);
            }
        }

        /// <summary>
        /// Updates the name of the group if the provided name is not null or whitespace.
        /// </summary>
        /// <param name="name"> The new name for the group.</param>
        public void UpdateName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Name = name;
            }
        }

        /// <summary>
        /// Updates the description of the group if the provided description is not null or whitespace.
        /// </summary>
        /// <param name="description"> The new description for the group.</param>
        public void UpdateDescription(string description)
        {
            if (!string.IsNullOrWhiteSpace(description))
            {
                Description = description;
            }
        }
    }
}
