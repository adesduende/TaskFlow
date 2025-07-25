using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.GroupCases.DeleteGroup
{
    public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, bool>
    {
        private readonly IGroupRepository _groupRepository;
        public DeleteGroupHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<bool> HandleAsync(DeleteGroupCommand request)
        {
            var group = await _groupRepository.GetByIdAsync(request.id);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {request.id} not found.");
            }
            await _groupRepository.DeleteAsync(request.id);
            return true;
        }
    }
}
