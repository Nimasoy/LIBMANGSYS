using Application.DTOs;
using MediatR;

namespace Application.Queries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public int Id { get; set; }
        public GetUserByIdQuery(int id) => Id = id;
    }

}
