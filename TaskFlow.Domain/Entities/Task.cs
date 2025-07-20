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
        public string Title { get; set; }
        public string Description { get; set; }
        public PriorityEnum Priority { get; set; }
        public StatusEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? TimeLimit { get; set; }
        public Guid? UserId { get; set; }

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
    }
}
