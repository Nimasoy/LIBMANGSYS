using Application.Commands.Books;
using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetBooksAsync();
        Task<BookDto> GetBookByIdAsync(int id);
        Task<int> AddBookAsync(AddBookCommand request);
        Task UpdateBookAsync(UpdateBookCommand request);
        Task DeleteBookAsync(int id);
        Task BorrowBookAsync(BorrowBookCommand request);
        Task ReturnBookAsync(ReturnBookCommand request);
        Task ReserveBookAsync(ReserveBookCommand request);
        Task<IEnumerable<BookDto>> GetMostBorrowedAsync(int count);
        Task<IEnumerable<BookDto>> GetLeastBorrowedAsync(int count);
        Task<IEnumerable<BookDto>> GetBorrowedBooksByUserIdAsync(int userId);
        Task<IEnumerable<OverdueDto>> GetOverdueBooksAsync();

    }

}
