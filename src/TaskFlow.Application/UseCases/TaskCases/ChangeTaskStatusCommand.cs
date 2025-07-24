using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.TaskCases
{
    public record ChangeTaskStatusCommand (
            Guid taskId,
            Domain.Enums.StatusEnum status
        ) : IRequest<bool>;
}
