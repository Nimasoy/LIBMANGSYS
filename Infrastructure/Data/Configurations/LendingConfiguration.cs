using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Configurations
{
    public class LendingConfiguration : IEntityTypeConfiguration<Lending>
    {
        public void Configure(EntityTypeBuilder<Lending> builder)
        {
            builder.HasKey(l => l.Id);

            builder.HasOne(l => l.Book)
                .WithMany(b => b.Lendings)
                .HasForeignKey(l => l.BookId);

            builder.HasOne(l => l.User)
                .WithMany(u => u.BorrowedBooks)
                .HasForeignKey(l => l.UserId);
        }
    }

}
