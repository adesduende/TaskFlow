using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.GroupCases.CreateGroup
{
    public record CreateGroupCommand(
        string name,
        string description,
        List<Guid>? users,
        List<Guid>? tasks
        ) : IRequest<Guid>;
}
