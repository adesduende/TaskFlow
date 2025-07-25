
using TaskFlow.Domain.Aggregates;

namespace TaskFlow.Domain.Repositories
{
    public interface IGroupRepository
    {
        /// <summary>
        /// Adds a new group to the repository.
        /// </summary>
        /// <param name="group">The group to add.</param>
        Task<Guid> AddAsync(Group group);
        /// <summary>
        /// Retrieves a group by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the group.</param>
        Task<Group?> GetByIdAsync(Guid id);
        /// <summary>
        /// Updates an existing group in the repository.
        /// </summary>
        /// <param name="group">The group to update.</param>
        Task UpdateAsync(Group group);
        /// <summary>
        /// Deletes a group from the repository.
        /// </summary>
        /// <param name="id">The unique identifier of the group to delete.</param>
        Task DeleteAsync(Guid id);
    }
}
