using Catalog.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Seeds.Categories
{
    public class CategorySeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public CategorySeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 1;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Categories.AnyAsync(cancellationToken))
            {
                return;
            }

            List<Category> categories =
            [
                new Category("Novela", "Obras narrativas de ficción de larga extensión"),
                new Category("Ciencia ficción", "Relatos que exploran futuros, tecnologías y sociedades imaginadas"),
                new Category("Tecnología", "Libros sobre desarrollo de software, arquitectura e ingeniería"),
                new Category("Historia", "Obras sobre el pasado y la evolución de la humanidad"),
            ];

            await _context.Categories.AddRangeAsync(categories, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
