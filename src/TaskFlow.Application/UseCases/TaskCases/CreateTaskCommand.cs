using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.TaskCases
{
    public record CreateTaskCommand(
            string title,
            string description,
            Domain.Enums.PriorityEnum priority,
            DateTime? timeLimit,
            Guid? userId,
            Domain.Enums.StatusEnum status = Domain.Enums.StatusEnum.NotStarted
        ) : IRequest<Guid>;
}
