using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.GroupCases.DeleteGroup
{
    public record DeleteGroupCommand(Guid id) : IRequest<bool>;
}
