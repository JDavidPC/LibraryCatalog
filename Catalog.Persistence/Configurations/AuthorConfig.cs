using Catalog.Domain.Entities.Authors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Configurations
{
    internal class AuthorConfig : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FirstName)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(a => a.LastName)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(a => a.Nationality)
                   .HasMaxLength(64);

            builder.Ignore(a => a.FullName);
        }
    }
}
