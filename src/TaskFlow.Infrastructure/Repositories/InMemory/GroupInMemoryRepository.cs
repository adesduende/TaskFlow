using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Domain.Aggregates;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Infrastructure.Repositories.InMemory
{
    public class GroupInMemoryRepository : IGroupRepository
    {
        private readonly List<Group> _groups;
        public GroupInMemoryRepository()
        {
            _groups = new List<Group>();
        }
        public async Task<Guid> AddAsync(Group group)
        {
            _groups.Add(group);

            return group.Id;
        }
        public async Task DeleteAsync(Guid id)
        {
            _groups.RemoveAll(g => g.Id == id);
        }
        public async Task<Group?> GetByIdAsync(Guid id)
        {
            var group = _groups.FirstOrDefault(g => g.Id == id);

            if (group == null)
            {
                return null;
            }
            return group;
        }
        public async Task UpdateAsync(Group group)
        {
            var existingGroup = _groups.FirstOrDefault(g => g.Id == group.Id);
            if (existingGroup != null)
            {
                existingGroup = group;
            }
            else
            {
                throw new KeyNotFoundException($"Group with ID {group.Id} not found.");
            }
        }
    }
}
