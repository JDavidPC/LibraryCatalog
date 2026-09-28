using Catalog.Domain.Entities.Books;

namespace Catalog.Application.UseCases.Books.Common
{
    internal static class MapperExtensions
    {
        public static BookListItemDTO ToListItemDTO(this Book book)
        {
            return new BookListItemDTO
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn.Value,
                PublicationYear = book.PublicationYear,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.FullName,
                CategoryId = book.CategoryId,
                CategoryName = book.Category.Name,
            };
        }
    }
}
