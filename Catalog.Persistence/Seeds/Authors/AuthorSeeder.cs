using Catalog.Domain.Entities.Authors;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Seeds.Authors
{
    public class AuthorSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public AuthorSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 2;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Authors.AnyAsync(cancellationToken))
            {
                return;
            }

            List<Author> authors =
            [
                new Author("Gabriel", "García Márquez", "Colombiana"),
                new Author("George", "Orwell", "Británica"),
                new Author("Frank", "Herbert", "Estadounidense"),
                new Author("Isaac", "Asimov", "Estadounidense"),
                new Author("Robert C.", "Martin", "Estadounidense"),
                new Author("Eric", "Evans", "Estadounidense"),
                new Author("Yuval Noah", "Harari", "Israelí"),
            ];

            await _context.Authors.AddRangeAsync(authors, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
