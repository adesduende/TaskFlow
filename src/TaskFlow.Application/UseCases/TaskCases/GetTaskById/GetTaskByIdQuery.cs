using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.TaskCases.GetTaskById
{
    public record GetTaskByIdQuery (Guid taskId) : IRequest<TaskDTO>;
}
