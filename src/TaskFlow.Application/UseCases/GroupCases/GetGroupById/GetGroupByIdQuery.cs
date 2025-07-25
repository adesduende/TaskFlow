using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.GroupCases.GetGroupById
{
    public record GetGroupByIdQuery(Guid id) : IRequest<GroupDTO>;
}
