using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Domain.Entities
{
    public class User : Entity
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public User(Guid id, string name, string email)
            : base(id)
        {
            Name = name;
            Email = email;
        }
    }
}
