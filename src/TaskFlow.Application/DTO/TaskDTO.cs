using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTO
{
    public class TaskDTO
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? TimeLimit { get; set; }
        public string UserId { get; set; }
        public TaskDTO(string id, string title, string description, string priority, string status, DateTime createdAt, DateTime? timeLimit, string userId)
        {
            Id = id;
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
