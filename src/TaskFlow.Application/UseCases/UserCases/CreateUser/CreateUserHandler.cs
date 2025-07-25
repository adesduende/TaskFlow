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

        public CreateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<Guid> HandleAsync(CreateUserCommand request)
        {
            if (string.IsNullOrWhiteSpace(request.name))
                throw new ArgumentException("Name cannot be empty.", nameof(request.name));
            if (string.IsNullOrWhiteSpace(request.email))
                throw new ArgumentException("Email cannot be empty.", nameof(request.email));
            if (string.IsNullOrWhiteSpace(request.password))
                throw new ArgumentException("Password cannot be empty.", nameof(request.password));

            var user = new Domain.Entities.User(Guid.NewGuid(), request.name, request.email);

            await _userRepository.AddUserAsync(user);
            //!TODO: Implement password hashing and storage
            return user.Id;
        }
    }
}
