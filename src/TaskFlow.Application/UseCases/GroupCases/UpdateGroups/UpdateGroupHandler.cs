using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.GroupCases.UpdateGroups
{
    public class UpdateGroupHandler : IRequestHandler<UpdateGroupCommand, Guid>
    {
        private readonly IGroupRepository _groupRepository;

        public UpdateGroupHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository ?? throw new ArgumentNullException(nameof(groupRepository));
        }

        public async Task<Guid> HandleAsync(UpdateGroupCommand request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var group = await _groupRepository.GetByIdAsync(request.Id);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {request.Id} not found.");
            }
            var tempGroup = new Domain.Aggregates.Group(request.Id, request.Name, request.Description, request.Users, request.Tasks);

            await _groupRepository.UpdateAsync(tempGroup);

            return group.Id;
        }
    }
}
