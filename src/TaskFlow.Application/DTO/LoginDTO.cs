using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskFlow.Application.DTO
{
    public class LoginDTO
    {
        public required string Token { get; set; }
        public required string UserId { get; set; }
    }
}
