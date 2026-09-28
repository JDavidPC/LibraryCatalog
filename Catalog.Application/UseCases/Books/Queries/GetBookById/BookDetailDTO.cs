namespace Catalog.Application.UseCases.Books.Queries.GetBookById
{
    public class BookDetailDTO
    {
        public Guid Id { get; init; }
        public string Title { get; init; } = null!;
        public string Isbn { get; init; } = null!;
        public int PublicationYear { get; init; }
        public string? Description { get; init; }
        public Guid AuthorId { get; init; }
        public string AuthorName { get; init; } = null!;
        public string? AuthorNationality { get; init; }
        public Guid CategoryId { get; init; }
        public string CategoryName { get; init; } = null!;
        public string? CategoryDescription { get; init; }
    }
}
