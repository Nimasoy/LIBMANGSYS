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
using Application.Commands;

namespace LibTest
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BookService>> _loggerMock;
        private readonly Mock<ICacheService> _cacheMock;
        private readonly BookService _bookService;

        public BookServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BookService>>();
            _cacheMock = new Mock<ICacheService>();
            _bookService = new BookService(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object, _cacheMock.Object);
        }

        [Fact]
        public async Task GetBooksAsync_ShouldReturnBooksFromCache_WhenCacheExists()
        {
            // Arrange
            var cachedBooks = new List<BookDto>
            {
                new() { Title = "Test Book 1", Author = "Author 1", InventoryCount = 1, CategoryName = "Fiction", Tags = new List<string>(), Status = "Available" },
                new() { Title = "Test Book 2", Author = "Author 2", InventoryCount = 2, CategoryName = "Non-Fiction", Tags = new List<string>(), Status = "Available" }
            };

            _cacheMock.Setup(x => x.GetAsync<IEnumerable<BookDto>>("book_list"))
                .ReturnsAsync(cachedBooks);

            // Act
            var result = await _bookService.GetBooksAsync();

            // Assert
            result.Should().BeEquivalentTo(cachedBooks);
            _unitOfWorkMock.Verify(x => x.Books.GetBooksAsync(), Times.Never);
        }

        [Fact]
        public async Task GetBooksAsync_ShouldReturnBooksFromDatabase_WhenCacheDoesNotExist()
        {
            // Arrange
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
                .ReturnsAsync(new List<BookDto>());

            _unitOfWorkMock.Setup(x => x.Books.GetBooksAsync())
                .ReturnsAsync(books);

            _mapperMock.Setup(x => x.Map<IEnumerable<BookDto>>(books))
                .Returns(bookDtos);

            // Act
            var result = await _bookService.GetBooksAsync();

            // Assert
            result.Should().BeEquivalentTo(bookDtos);
            _cacheMock.Verify(x => x.SetAsync("book_list", bookDtos, null), Times.Once);
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldThrowException_WhenBookNotFound()
        {
            // Arrange
            var command = new BorrowBookCommand(1, 1);
            _unitOfWorkMock.Setup(x => x.Books.GetBookByIdAsync(1))
                .ReturnsAsync((Book?)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookService.BorrowBookAsync(command));
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldThrowException_WhenBookNotAvailable()
        {
            // Arrange
            var command = new BorrowBookCommand(1, 1);
            var book = new Book
            {
                Id = 1, Title = "Some Title", Author = "Some Author", Status = BookStatus.Available, InventoryCount = 0, Category = new Category { Id = 1, Name = "Test Category" }
            }; var user = new User { Id = 1, MaxBorrowLimit = 5 };
            _unitOfWorkMock.Setup(x => x.Books.GetBookByIdAsync(1))
                .ReturnsAsync(book);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookService.BorrowBookAsync(command));
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldThrowException_WhenNoCopiesAvailable()
        {
            // Arrange
            var command = new BorrowBookCommand(1, 1);
            var book = new Book { Id = 1,
                Title = "Some Title", Author = "Some Author",Status = BookStatus.Available, InventoryCount = 0,
                Category = new Category { Id = 1, Name = "Test Category" }
            };
            var user = new User { Id = 1, MaxBorrowLimit = 5 };

            _unitOfWorkMock.Setup(x => x.Books.GetBookByIdAsync(1))
                .ReturnsAsync(book);
            _unitOfWorkMock.Setup(x => x.Users.GetUserByIdAsync(1))
                .ReturnsAsync(user);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _bookService.BorrowBookAsync(command));
        }

        [Fact]
        public async Task BorrowBookAsync_ShouldSucceed_WhenBookIsAvailable()
        {
            // Arrange
            var command = new BorrowBookCommand(1, 1);
            var book = new Book
            {
                Id = 1, Title = "Some Title", Author = "Some Author", Status = BookStatus.Available, InventoryCount = 0, Category = new Category { Id = 1, Name = "Test Category" }
            }; var user = new User { Id = 1, MaxBorrowLimit = 5 };

            _unitOfWorkMock.Setup(x => x.Books.GetBookByIdAsync(1))
                .ReturnsAsync(book);
            _unitOfWorkMock.Setup(x => x.Users.GetUserByIdAsync(1))
                .ReturnsAsync(user);
            _unitOfWorkMock.Setup(x => x.Lendings.GetActiveBorrowedCountByUserAsync(1))
                .ReturnsAsync(0);

            // Act
            await _bookService.BorrowBookAsync(command);

            // Assert
            _unitOfWorkMock.Verify(x => x.Lendings.AddLendingAsync(It.IsAny<Lending>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.Books.UpdateBookAsync(It.IsAny<Book>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(), Times.Once);
            _cacheMock.Verify(x => x.RemoveAsync($"book_{book.Id}"), Times.Once);
            _cacheMock.Verify(x => x.RemoveAsync("book_list"), Times.Once);
        }
    }
}