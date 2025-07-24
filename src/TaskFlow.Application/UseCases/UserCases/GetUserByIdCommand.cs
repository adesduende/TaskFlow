
using System.Threading.Tasks;
using TaskFlow.Application.DTO;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.UseCases.UserCases
{
    public record GetUserByIdCommand ( Guid id) : IRequest<UserDTO>;
}
