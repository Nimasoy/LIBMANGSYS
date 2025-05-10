using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetTagsAsync();
        Task CreateTagAsync(string name);
        Task DeleteTagAsync(int id);
    }

}
