using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.UserCases.Login
{
    public record LoginCommand (string email, string password): IRequest<string>;    
}
