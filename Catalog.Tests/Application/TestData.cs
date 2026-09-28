using Catalog.Domain.Entities.Authors;
using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Books.ValueObjects;
using Catalog.Domain.Entities.Categories;

namespace Catalog.Tests.Application
{
    internal static class TestData
    {
        public static Author CreateAuthor(string firstName = "Gabriel", string lastName = "García Márquez")
        {
            return new Author(firstName, lastName, "Colombiana");
        }

        public static Category CreateCategory(string name = "Novela")
        {
            return new Category(name, "Descripción de la categoría");
        }

        // EF Core carga las propiedades de navegación al consultar la base de datos.
        // En las pruebas se asignan por reflexión porque el dominio las expone con setter privado.
        public static Book CreateBook(string title, string isbn, int year, Author author, Category category)
        {
            Book book = new(title, new Isbn(isbn), year, "Descripción del libro", author.Id, category.Id);

            typeof(Book).GetProperty(nameof(Book.Author))!.SetValue(book, author);
            typeof(Book).GetProperty(nameof(Book.Category))!.SetValue(book, category);

            return book;
        }
    }
}
