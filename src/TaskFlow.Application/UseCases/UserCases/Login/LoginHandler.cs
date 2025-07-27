using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Repositories;

namespace TaskFlow.Application.UseCases.UserCases.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        public LoginHandler(
            IPasswordHasher passwordHasher, 
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }
        public async Task<string> HandleAsync(LoginCommand request)
        {
            var user = await _userRepository.GetUserByEmailAsync(request.email);
            if (user == null || !_passwordHasher.VerifyPassword(user.Password, request.password))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            // Generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);
            return token;
        }
    }
}
