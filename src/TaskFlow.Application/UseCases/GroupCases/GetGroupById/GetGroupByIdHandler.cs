
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Mappers;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.GroupCases.GetGroupById
{
    public class GetGroupByIdHandler : IRequestHandler<GetGroupByIdQuery, GroupDTO>
    {
        private readonly IGroupRepository _groupRepository;

        public GetGroupByIdHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<GroupDTO> HandleAsync(GetGroupByIdQuery request)
        {
            var group = await _groupRepository.GetByIdAsync(request.id);
            if (group == null)
            {
                throw new KeyNotFoundException($"Group with ID {request.id} not found.");
            }
            return group.ToDto();
        }
    }
}
