using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.UseCases.TaskCases
{
    public record GetAllTasksCommand (
            Domain.Enums.StatusEnum? status = null,
            Domain.Enums.PriorityEnum? priority = null,
            User? user = null,
            DateTime? timeLimit = null,
            DateTime? createdAt = null
        ) : IRequest<IEnumerable<TaskDTO>>;
}
