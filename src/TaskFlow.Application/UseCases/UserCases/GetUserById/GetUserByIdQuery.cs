
using System.Threading.Tasks;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.UserCases.GetUserById
{
    public record GetUserByIdQuery ( Guid id) : IRequest<UserDTO>;
}
