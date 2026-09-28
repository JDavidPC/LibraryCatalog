using Catalog.Domain.Entities.Books;

namespace Catalog.Application.UseCases.Books.Queries.GetBookById
{
    internal static class MapperExtensions
    {
        public static BookDetailDTO ToDetailDTO(this Book book)
        {
            return new BookDetailDTO
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn.Value,
                PublicationYear = book.PublicationYear,
                Description = book.Description,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.FullName,
                AuthorNationality = book.Author.Nationality,
                CategoryId = book.CategoryId,
                CategoryName = book.Category.Name,
                CategoryDescription = book.Category.Description,
            };
        }
    }
}
