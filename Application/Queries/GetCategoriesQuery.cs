using MediatR;
using Application.DTOs;
namespace Application.Queries
{
    public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>> { }
}