using Catalog.Domain.Entities.Authors;
using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

            base.OnModelCreating(builder);
        }
    }
}
