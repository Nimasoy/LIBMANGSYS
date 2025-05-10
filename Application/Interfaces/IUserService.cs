using Application.Commands;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task<string> LoginAsync(CreateLoginCommand request);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> CreateUserAsync(CreateUserCommand request);
        Task UpdateUserAsync(UpdateUserCommand request);
        Task DeleteUserAsync(int id);
        Task<IEnumerable<BorrowingHistoryDto>> GetUserBorrowingHistoryAsync(int userId);
        Task<IEnumerable<UserOverdueDto>> GetUserOverdueBooksAsync(int userId);

    }

}
