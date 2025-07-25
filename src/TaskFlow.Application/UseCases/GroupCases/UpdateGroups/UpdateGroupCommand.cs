
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.GroupCases.UpdateGroups
{
    public record UpdateGroupCommand(
        Guid Id,
        string Name,
        string Description,
        List<Guid> Users,
        List<Guid> Tasks
    ) : IRequest<Guid>;
}
