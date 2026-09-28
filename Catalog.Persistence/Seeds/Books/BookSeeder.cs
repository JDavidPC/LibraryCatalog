using Catalog.Domain.Entities.Authors;
using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Books.ValueObjects;
using Catalog.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Seeds.Books
{
    public class BookSeeder : IDataSeeder
    {
        private readonly DataContext _context;

        public BookSeeder(DataContext context)
        {
            _context = context;
        }

        public int Order => 3;

        public async Task SeedAsync(CancellationToken cancellationToken = default)
        {
            if (await _context.Books.AnyAsync(cancellationToken))
            {
                return;
            }

            List<Author> authorsList = await _context.Authors.ToListAsync(cancellationToken);
            List<Category> categoriesList = await _context.Categories.ToListAsync(cancellationToken);

            Dictionary<string, Author> authors = authorsList.ToDictionary(a => a.LastName);
            Dictionary<string, Category> categories = categoriesList.ToDictionary(c => c.Name);

            List<Book> books =
            [
                new Book("Cien años de soledad",
                         new Isbn("9780307474728"),
                         1967,
                         "La historia de la familia Buendía a lo largo de siete generaciones en el pueblo de Macondo.",
                         authors["García Márquez"].Id,
                         categories["Novela"].Id),

                new Book("El amor en los tiempos del cólera",
                         new Isbn("9780307387264"),
                         1985,
                         "Una historia de amor que perdura más de cincuenta años entre Florentino Ariza y Fermina Daza.",
                         authors["García Márquez"].Id,
                         categories["Novela"].Id),

                new Book("1984",
                         new Isbn("9780451524935"),
                         1949,
                         "Distopía sobre un Estado totalitario que vigila y controla cada aspecto de la vida.",
                         authors["Orwell"].Id,
                         categories["Ciencia ficción"].Id),

                new Book("Dune",
                         new Isbn("9780441172719"),
                         1965,
                         "Épica de ciencia ficción ambientada en el desértico planeta Arrakis.",
                         authors["Herbert"].Id,
                         categories["Ciencia ficción"].Id),

                new Book("Fundación",
                         new Isbn("9780553293357"),
                         1951,
                         "Un matemático predice la caída de un imperio galáctico y crea un plan para acortar la oscuridad posterior.",
                         authors["Asimov"].Id,
                         categories["Ciencia ficción"].Id),

                new Book("Clean Code",
                         new Isbn("9780132350884"),
                         2008,
                         "Guía de buenas prácticas para escribir código legible, mantenible y expresivo.",
                         authors["Martin"].Id,
                         categories["Tecnología"].Id),

                new Book("Clean Architecture",
                         new Isbn("9780134494166"),
                         2017,
                         "Principios de diseño para construir sistemas desacoplados y fáciles de evolucionar.",
                         authors["Martin"].Id,
                         categories["Tecnología"].Id),

                new Book("Domain-Driven Design",
                         new Isbn("9780321125217"),
                         2003,
                         "Enfoque para modelar software complejo a partir del dominio del negocio.",
                         authors["Evans"].Id,
                         categories["Tecnología"].Id),

                new Book("Sapiens: De animales a dioses",
                         new Isbn("9780062316097"),
                         2014,
                         "Recorrido por la historia de la humanidad desde la aparición del Homo sapiens hasta hoy.",
                         authors["Harari"].Id,
                         categories["Historia"].Id),

                new Book("Homo Deus",
                         new Isbn("9780062464316"),
                         2016,
                         "Reflexión sobre el futuro de la humanidad frente a la tecnología y los datos.",
                         authors["Harari"].Id,
                         categories["Historia"].Id),
            ];

            await _context.Books.AddRangeAsync(books, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
