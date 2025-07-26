using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.GroupCases.CreateGroup
{
    public class CreateGroupHandler : IRequestHandler<CreateGroupCommand, Guid>
    {
        private readonly IGroupRepository _groupRepository;

        public CreateGroupHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task<Guid> HandleAsync(CreateGroupCommand request)
        {
            var result = await _groupRepository.AddAsync(
                new Domain.Aggregates.Group(
                    Guid.NewGuid(),
                    request.name,
                    request.description,
                    request.users ?? new List<Guid>(),
                    request.tasks ?? new List<Guid>(),
                    request.ownerId
                ));

            return result;
        }
    }
}
