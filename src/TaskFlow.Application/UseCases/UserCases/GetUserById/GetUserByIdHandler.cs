using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.UserCases.GetUserById
{
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdCommand , UserDTO>
    {
        private readonly IUserRepository _userRepository;

        public GetUserByIdHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        public async Task<UserDTO> HandleAsync(GetUserByIdCommand request)
        {
            if (request.id == Guid.Empty)
                throw new ArgumentException("User ID cannot be empty.", nameof(request.id));
            var user = await _userRepository.GetUserByIdAsync(request.id);
            if (user == null)
                throw new KeyNotFoundException("User not found.");
            return Mappers.UserMapper.ToDto(user);
        }
    }
}
