using Catalog.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Persistence.Configurations
{
    internal class CategoryConfig : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .HasMaxLength(64)
                   .IsRequired();

            builder.Property(c => c.Description)
                   .HasMaxLength(512);

            builder.HasIndex(c => c.Name)
                   .IsUnique();
        }
    }
}
