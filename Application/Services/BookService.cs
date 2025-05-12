using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Polly.Retry;
using Microsoft.Data.SqlClient;
using Application.Commands.Books;

namespace Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepo;
        private readonly ITagRepository _tagRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILendingRepository _lendingRepo;
        private readonly IReservationRepository _reservationRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;
        private readonly ICacheService _cache;
        private readonly ResiliencePipelineProvider<string> _pipelineProvider;

        public BookService(
         IBookRepository bookRepo,
         ITagRepository tagRepo,
         IUserRepository userRepo,
         ILendingRepository lendingRepo,
         IReservationRepository reservationRepo,
         IUnitOfWork unitOfWork,
         IMapper mapper,
         ILogger<BookService> logger,
         ICacheService cache,
         ResiliencePipelineProvider<string> pipelineProvider)
        {
            _bookRepo = bookRepo;
            _tagRepo = tagRepo;
            _userRepo = userRepo;
            _lendingRepo = lendingRepo;
            _reservationRepo = reservationRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
            _pipelineProvider = pipelineProvider;
        }

        public async Task<IEnumerable<BookDto>> GetBooksAsync()
        {
            var pipeline = _pipelineProvider.GetPipeline("read-pipeline");
            return await pipeline.ExecuteAsync(async _ => 
            {
                const string key = "book_list";
                var cached = await _cache.GetAsync<IEnumerable<BookDto>>(key);
                if (cached is not null)
                {
                    _logger.LogInformation("All books returned from cache");
                    return cached;
                }

                var books = await _bookRepo.GetBooksAsync();
                var dtos = _mapper.Map<IEnumerable<BookDto>>(books);
                await _cache.SetAsync(key, dtos);
                return dtos;
            });
        }

        public async Task<BookDto> GetBookByIdAsync(int id)
        {
            var pipeline = _pipelineProvider.GetPipeline("read-pipeline");
            return await pipeline.ExecuteAsync(async _ =>
            {
                var key = $"book_{id}";
                var cached = await _cache.GetAsync<BookDto>(key);
                if (cached is not null)
                {
                    _logger.LogInformation("Book {Id} returned from cache", id);
                    return cached;
                }

                var book = await _bookRepo.GetBookByIdAsync(id);
                if (book == null) throw new Exception("Book not found");

                var dto = _mapper.Map<BookDto>(book);
                await _cache.SetAsync(key, dto);
                return dto;
            });
        }

        public async Task<int> AddBookAsync(AddBookCommand request)
        {
            var book = _mapper.Map<Book>(request);

            book.InventoryCount = request.InventoryCount;

            var tags = await _tagRepo.GetTagsByIdsAsync(request.TagIds);
            book.Tags = tags.ToList();

            await _bookRepo.AddBookAsync(book);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Book added: {Title}", book.Title);
            await _cache.RemoveAsync("book_list");

            return book.Id;
        }


        public async Task UpdateBookAsync(UpdateBookCommand request)
        {
            var book = await _bookRepo.GetBookByIdAsync(request.Id);
            if (book == null)
            {
                _logger.LogWarning("Update failed: Book not found. Id={Id}", request.Id);
                throw new Exception("Book not found.");
            }

            book.Title = request.Title;
            book.Author = request.Author;
            book.CategoryId = request.CategoryId;
            var tags = await _tagRepo.GetTagsByIdsAsync(request.TagIds);
            book.Tags = tags.ToList();
            await _bookRepo.UpdateBookAsync(book);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Book updated: Id={Id}", book.Id);
            await _cache.RemoveAsync($"book_{book.Id}");
            await _cache.RemoveAsync("book_list");
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _bookRepo.GetBookByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Delete failed: Book not found. Id={Id}", id);
                throw new Exception("Book not found.");
            }
            await _bookRepo.DeleteBookAsync(book);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Book deleted: Id={Id}", book.Id);
            await _cache.RemoveAsync($"book_{id}");
            await _cache.RemoveAsync("book_list");
        }

        public async Task BorrowBookAsync(BorrowBookCommand request)
        {
            var pipeline = _pipelineProvider.GetPipeline("write-pipeline");
            await pipeline.ExecuteAsync(async _ =>
            {
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    var book = await _bookRepo.GetBookByIdAsync(request.BookId);
                    if (book == null)
                    {
                        _logger.LogWarning("Borrow failed: Book not found. BookId={Id}", request.BookId);
                        throw new Exception("Book not found.");
                    }

                    if (book.InventoryCount <= 0)
                    {
                        _logger.LogWarning("Borrow failed: No copies available. BookId={Id}", request.BookId);
                        throw new Exception("No copies available.");
                    }

                    var user = await _userRepo.GetUserByIdAsync(request.UserId);
                    if (user == null) throw new Exception("User not found.");

                    var userBorrowedCount = await _lendingRepo.GetActiveBorrowedCountByUserAsync(user.Id);
                    if (userBorrowedCount >= user.MaxBorrowLimit)
                        throw new Exception("User has reached the maximum borrow limit.");

                    var lending = new Lending
                    {
                        BookId = book.Id,
                        UserId = user.Id,
                        Book = book,
                        User = user,
                        BorrowedAt = DateTime.UtcNow,
                        DueAt = DateTime.UtcNow.AddDays(14)
                    };

                    book.InventoryCount--;

                    if (book.InventoryCount == 0)
                    {
                        book.Status = BookStatus.Borrowed;
                    }

                    await _lendingRepo.AddLendingAsync(lending);
                    await _bookRepo.UpdateBookAsync(book);

                    await _cache.RemoveAsync($"book_{book.Id}");
                    await _cache.RemoveAsync("book_list");

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();
                    _logger.LogInformation("Book borrowed: BookId={BookId}, UserId={UserId}", book.Id, request.UserId);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    _logger.LogError(ex, "Borrow failed: Transaction rolled back.");
                    throw;
                }
            });
        }

        public async Task ReturnBookAsync(ReturnBookCommand request)
        {
            var pipeline = _pipelineProvider.GetPipeline("write-pipeline");
            await pipeline.ExecuteAsync(async _ =>
            {
                await _unitOfWork.BeginTransactionAsync();
                try
                {
                    var lending = await _lendingRepo.GetActiveLendingAsync(request.BookId, request.UserId);
                    if (lending == null) throw new Exception("Lending not found.");

                    lending.ReturnedAt = DateTime.UtcNow;
                    lending.Book.InventoryCount++;

                    var activeReservations = await _reservationRepo.GetActiveReservationsForBookAsync(lending.BookId);
                    var oldestReservation = activeReservations.OrderBy(r => r.ReservedAt).FirstOrDefault();

                    if (oldestReservation != null)
                    {
                        var newLending = new Lending
                        {
                            BookId = lending.BookId,
                            UserId = oldestReservation.UserId,
                            Book = lending.Book,
                            User = oldestReservation.User,
                            BorrowedAt = DateTime.UtcNow,
                            DueAt = DateTime.UtcNow.AddDays(14)
                        };

                        await _reservationRepo.RemoveReservationAsync(oldestReservation);

                        await _lendingRepo.AddLendingAsync(newLending);

                        lending.Book.Status = BookStatus.Borrowed;
                        lending.Book.InventoryCount--;

                        _logger.LogInformation(
                            "Book automatically borrowed by reserved user: BookId={BookId}, UserId={UserId}",
                            lending.BookId,
                            oldestReservation.UserId);
                    }
                    else
                    {
                        lending.Book.Status = BookStatus.Available;
                    }

                    await _lendingRepo.UpdateLending(lending);
                    await _bookRepo.UpdateBookAsync(lending.Book);

                    await _cache.RemoveAsync($"book_{request.BookId}");
                    await _cache.RemoveAsync("book_list");

                    await _unitOfWork.SaveChangesAsync();
                    await _unitOfWork.CommitTransactionAsync();

                    _logger.LogInformation("Book returned: BookId={BookId}, UserId={UserId}", request.BookId, request.UserId);
                }
                catch (Exception ex)
                {
                    await _unitOfWork.RollbackTransactionAsync();
                    _logger.LogError(ex, "Return failed: Transaction rolled back.");
                    throw;
                }
            });
        }

        public async Task ReserveBookAsync(ReserveBookCommand request)
        {
            var user = await _userRepo.GetUserByIdAsync(request.UserId);
            if (user == null) throw new Exception("User not found.");
            var book = await _bookRepo.GetBookByIdAsync(request.BookId);
            if (book == null || book.Status == BookStatus.Available)
                throw new Exception("Book is available or not found.");

            var activeBorrowCount = await _lendingRepo.GetActiveBorrowedCountByUserAsync(user.Id);
            if (activeBorrowCount >= user.MaxBorrowLimit)
                throw new Exception("User has reached borrow limit.");

            var reservation = new Reservation
            {
                BookId = book.Id,
                Book = book,
                User = user,
                UserId = user.Id,
                ReservedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _reservationRepo.AddReservationAsync(reservation);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Book reserved: BookId={BookId}, UserId={UserId}", request.BookId, request.UserId);
        }

        public async Task<IEnumerable<BookDto>> GetMostBorrowedAsync(int count)
        {
            var books = await _bookRepo.GetMostBorrowedAsync(count);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<IEnumerable<BookDto>> GetLeastBorrowedAsync(int count)
        {
            var books = await _bookRepo.GetLeastBorrowedAsync(count);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<IEnumerable<BookDto>> GetBorrowedBooksByUserIdAsync(int userId)
        {
            var books = await _bookRepo.GetBorrowedBooksByUserId(userId);
            return _mapper.Map<IEnumerable<BookDto>>(books);
        }

        public async Task<IEnumerable<OverdueDto>> GetOverdueBooksAsync()
        {
            var lendings = await _lendingRepo.GetOverdueLendingsAsync();
            return _mapper.Map<IEnumerable<OverdueDto>>(lendings);
        }

    }
}
