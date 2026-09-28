using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Books.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Configurations
{
    internal class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.Property(b => b.PublicationYear)
                   .IsRequired();

            builder.Property(b => b.Description)
                   .HasMaxLength(2048);

            builder.OwnsOne(b => b.Isbn, isbn =>
            {
                isbn.Property(i => i.Value)
                    .HasColumnName("Isbn")
                    .HasMaxLength(Isbn.LENGTH)
                    .IsFixedLength()
                    .IsUnicode(false)
                    .IsRequired();

                isbn.HasIndex(i => i.Value)
                    .IsUnique();
            });

            builder.Navigation(b => b.Isbn)
                   .IsRequired();

            builder.HasOne(b => b.Author)
                   .WithMany(a => a.Books)
                   .HasForeignKey(b => b.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Category)
                   .WithMany(c => c.Books)
                   .HasForeignKey(b => b.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
