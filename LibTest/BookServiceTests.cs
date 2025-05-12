using Xunit;
using Moq;
using FluentAssertions;
using Application.Services;
using Domain.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Domain.Enums;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Application.Commands.Books;
using Polly.Registry;
using System.Runtime.CompilerServices;

namespace LibTest
{
    public class BookServiceTests
    {
        private readonly Mock<IBookRepository> _bookRepoMock;
        private readonly Mock<ITagRepository> _tagRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<ILendingRepository> _lendingRepoMock;
        private readonly Mock<IReservationRepository> _reservationRepoMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BookService>> _loggerMock;
        private readonly Mock<ICacheService> _cacheMock;
        private readonly BookService _bookService;
        private readonly Mock<ResiliencePipelineProvider<string>> _pipelineMock;

        public BookServiceTests()
        {
            _bookRepoMock = new Mock<IBookRepository>();
            _tagRepoMock = new Mock<ITagRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _lendingRepoMock = new Mock<ILendingRepository>();
            _reservationRepoMock = new Mock<IReservationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BookService>>();
            _cacheMock = new Mock<ICacheService>();
            _pipelineMock = new Mock<ResiliencePipelineProvider<string>>();

            _bookService = new BookService(
                _bookRepoMock.Object,
                _tagRepoMock.Object,
                _userRepoMock.Object,
                _lendingRepoMock.Object,
                _reservationRepoMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _loggerMock.Object,
                _cacheMock.Object,
                _pipelineMock.Object);
        }

        [Fact]
        public async Task GetBooksAsync_ShouldReturnBooksFromCache_WhenCacheExists()
        {
            var cachedBooks = new List<BookDto>
            {
                new() { Title = "Test Book 1", Author = "Author 1", InventoryCount = 1, CategoryName = "Fiction", Tags = new List<string>(), Status = "Available" },
                new() { Title = "Test Book 2", Author = "Author 2", InventoryCount = 2, CategoryName = "Non-Fiction", Tags = new List<string>(), Status = "Available" }
            };

            _cacheMock.Setup(x => x.GetAsync<IEnumerable<BookDto>>("book_list"))
                .ReturnsAsync(cachedBooks);

            var result = await _bookService.GetBooksAsync();

            result.Should().BeEquivalentTo(cachedBooks);
            _bookRepoMock.Verify(x => x.GetBooksAsync(), Times.Never);
        }

        [Fact]
        public async Task GetBooksAsync_ShouldReturnBooksFromDatabase_WhenCacheDoesNotExist()
        {
            var books = new List<Book>
            {
                new() { Id = 1, Title = "Test Book 1", Author = "Author 1", InventoryCount = 1, Status = BookStatus.Available,  Category = new Category { Id = 1, Name = "Fiction" } },
                new() { Id = 2, Title = "Test Book 2", Author = "Author 2", InventoryCount = 2, Status = BookStatus.Available, Category = new Category { Id = 2, Name = "Non-Fiction" }  }
            };

            var bookDtos = new List<BookDto>
            {
                new() { Title = "Test Book 1", Author = "Author 1", InventoryCount = 1, CategoryName = "Fiction", Tags = new List<string>(), Status = "Available" },
                new() { Title = "Test Book 2", Author = "Author 2", InventoryCount = 2, CategoryName = "Non-Fiction", Tags = new List<string>(), Status = "Available" }
            };

            _cacheMock.Setup(x => x.GetAsync<IEnumerable<BookDto>>("book_list"))
                .ReturnsAsync(null as IEnumerable<BookDto>);

            _bookRepoMock.Setup(x => x.GetBooksAsync())
                .ReturnsAsync(books);

            _mapperMock.Setup(x => x.Map<IEnumerable<BookDto>>(books))
                .Returns(bookDtos);

            var result = await _bookService.GetBooksAsync();

            result.Should().BeEquivalentTo(bookDtos);
            _cacheMock.Verify(x => x.SetAsync("book_list", bookDtos, null), Times.Once);
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldThrowException_WhenBookNotFound()
        {
            var command = new BorrowBookCommand(1, 1);
            _bookRepoMock.Setup(x => x.GetBookByIdAsync(1))
                .ReturnsAsync((Book?)null);

            await Assert.ThrowsAsync<Exception>(() => _bookService.BorrowBookAsync(command));
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldThrowException_WhenBookNotAvailable()
        {
            var command = new BorrowBookCommand(1, 1);
            var book = new Book
            {
                Id = 1,
                Title = "Some Title",
                Author = "Some Author",
                Status = BookStatus.Available,
                InventoryCount = 0,
                Category = new Category { Id = 1, Name = "Test Category" }
            };

            _bookRepoMock.Setup(x => x.GetBookByIdAsync(1)).ReturnsAsync(book);

            await Assert.ThrowsAsync<Exception>(() => _bookService.BorrowBookAsync(command));
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldThrowException_WhenNoCopiesAvailable()
        {
            var command = new BorrowBookCommand(1, 1);
            var book = new Book
            {
                Id = 1,
                Title = "Some Title",
                Author = "Some Author",
                Status = BookStatus.Available,
                InventoryCount = 0,
                Category = new Category { Id = 1, Name = "Test Category" } 
            };
            var user = new User { Id = 1, MaxBorrowLimit = 5 };

            _bookRepoMock.Setup(x => x.GetBookByIdAsync(1)).ReturnsAsync(book);
            _userRepoMock.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(user);

            await Assert.ThrowsAsync<Exception>(() => _bookService.BorrowBookAsync(command));
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldSucceed_WhenBookIsAvailable()
        {
            var command = new BorrowBookCommand(1, 1);
            var book = new Book
            {
                Id = 1,
                Title = "Some Title",
                Author = "Some Author",
                Status = BookStatus.Available,
                InventoryCount = 1,
                Category = new Category { Id = 1, Name = "Test Category" }
            };

            var user = new User { Id = 1, MaxBorrowLimit = 5 };

            _bookRepoMock.Setup(x => x.GetBookByIdAsync(1)).ReturnsAsync(book);
            _userRepoMock.Setup(x => x.GetUserByIdAsync(1)).ReturnsAsync(user);
            _lendingRepoMock.Setup(x => x.GetActiveBorrowedCountByUserAsync(1)).ReturnsAsync(0);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync()).ReturnsAsync(1);
            _unitOfWorkMock.Setup(x => x.CommitTransactionAsync()).Returns(Task.CompletedTask);

            await _bookService.BorrowBookAsync(command);

            _lendingRepoMock.Verify(x => x.AddLendingAsync(It.IsAny<Lending>()), Times.Once);
            _bookRepoMock.Verify(x => x.UpdateBookAsync(It.IsAny<Book>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(), Times.Once);
            _cacheMock.Verify(x => x.RemoveAsync($"book_{book.Id}"), Times.Once);
            _cacheMock.Verify(x => x.RemoveAsync("book_list"), Times.Once);
        }
    }
}
