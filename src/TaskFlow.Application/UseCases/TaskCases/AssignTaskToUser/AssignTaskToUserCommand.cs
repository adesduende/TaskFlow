using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.TaskCases.AssignTaskToUser
{
    public record AssignTaskToUserCommand (
            Guid taskId,
            Guid userId
        ) : IRequest<bool>;
}
