using MediatR;
using Application.DTOs;
namespace Application.Queries.Categories
{
    public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>> { }
}