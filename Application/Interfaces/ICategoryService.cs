using Application.Commands;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetCategoriesAsync();
        Task CreateCategoryAsync(CreateCategoryCommand request);
        Task UpdateCategoryAsync(UpdateCategoryCommand request);
        Task DeleteCategoryAsync(DeleteCategoryCommand request);
    }

}
