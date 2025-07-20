using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class Entity
    {
        public Guid Id { get; set; }

        public Entity(Guid id)
        {
            Id = id;
        }
        public Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
