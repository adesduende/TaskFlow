using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.UserCases.CreateUser
{
    public class CreateUserHandler : IRequestHandler<CreateUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public CreateUserHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public async Task<Guid> HandleAsync(CreateUserCommand request)
        {          
            var hashedPassword = _passwordHasher.HashPassword(request.password);
            var user = new Domain.Entities.User(Guid.NewGuid(), request.name, request.email, hashedPassword);

            await _userRepository.AddUserAsync(user);
            return user.Id;
        }
    }
}
