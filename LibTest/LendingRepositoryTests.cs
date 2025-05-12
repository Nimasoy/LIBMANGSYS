using Xunit;
using FluentAssertions;
using Infrastructure.Repositories;
using Infrastructure.Data;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace LibTest
{
    public class LendingRepositoryTests : IDisposable
    {
        private readonly LibraryDbContext _context;
        private readonly LendingRepository _repository;

        public LendingRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<LibraryDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new LibraryDbContext(options);
            _repository = new LendingRepository(_context);
        }

        [Fact]
        public async Task GetActiveBorrowedCountByUserAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Test Category" };

            var user = new User { Id = 1 };
            var book1 = new Book
            {
                Id = 1,
                Title = "Test Book",
                Author = "Test Author",
                Category = category,
                Status = BookStatus.Available,
                InventoryCount = 1
            };
            var book2 = new Book
            {
                Id = 2,
                Title = "Test Book 2",
                Author = "Test Author 2",
                Category = category,
                Status = BookStatus.Available,
                InventoryCount = 1
            };

            var lendings = new[]
            {
                new Lending
                {
                    UserId = 1,
                    BookId = 1,
                    Book = book1,
                    User = user,
                    BorrowedAt = DateTime.UtcNow,
                    DueAt = DateTime.UtcNow.AddDays(14)
                },
                new Lending
                {
                    UserId = 1,
                    BookId = 2,
                    Book = book2,
                    User = user,
                    BorrowedAt = DateTime.UtcNow,
                    DueAt = DateTime.UtcNow.AddDays(14),
                    ReturnedAt = DateTime.UtcNow
                }
            };

            await _context.Users.AddAsync(user);
            await _context.Books.AddRangeAsync(book1, book2);
            await _context.Lendings.AddRangeAsync(lendings);
            await _context.SaveChangesAsync();

            // Act
            var count = await _repository.GetActiveBorrowedCountByUserAsync(1);

            // Assert
            count.Should().Be(1);
        }

        [Fact]
        public async Task GetActiveLendingAsync_ShouldReturnCorrectLending()
        {
            // Arrange
            var category = new Category { Id = 1, Name = "Test Category" };

            var user = new User { Id = 1 };
            var book1 = new Book
            {
                Id = 1,
                Title = "Test Book",
                Author = "Test Author",
                Category = category,
                Status = BookStatus.Available,
                InventoryCount = 1
            };
            var book2 = new Book
            {
                Id = 2,
                Title = "Test Book 2",
                Author = "Test Author 2",
                Category = category,
                Status = BookStatus.Available,
                InventoryCount = 1
            };

            var lendings = new[]
            {
                new Lending
                {
                    UserId = 1,
                    BookId = 1,
                    Book = book1,
                    User = user,
                    BorrowedAt = DateTime.UtcNow,
                    DueAt = DateTime.UtcNow.AddDays(14)
                },
                new Lending
                {
                    UserId = 1,
                    BookId = 2,
                    Book = book2,
                    User = user,
                    BorrowedAt = DateTime.UtcNow,
                    DueAt = DateTime.UtcNow.AddDays(14),
                    ReturnedAt = DateTime.UtcNow
                }
            };

            await _context.Users.AddAsync(user);
            await _context.Books.AddRangeAsync(book1, book2);
            await _context.Lendings.AddRangeAsync(lendings);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetActiveLendingAsync(1, 1);

            // Assert
            result.Should().NotBeNull();
            result!.BookId.Should().Be(1);
            result.UserId.Should().Be(1);
            result.ReturnedAt.Should().BeNull();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}