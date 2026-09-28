using Catalog.Domain.Entities.Authors;
using Catalog.Domain.Entities.Books.ValueObjects;
using Catalog.Domain.Entities.Categories;
using Catalog.Domain.Exceptions;

namespace Catalog.Domain.Entities.Books
{
    public sealed class Book
    {
        public const int MIN_PUBLICATION_YEAR = 1000;

        public Guid Id { get; private set; }
        public string Title { get; private set; } = null!;
        public Isbn Isbn { get; private set; } = null!;
        public int PublicationYear { get; private set; }
        public string? Description { get; private set; }
        public Guid AuthorId { get; private set; }
        public Author Author { get; private set; } = null!;
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = null!;

        private Book()
        {

        }

        public Book(string title,
                    Isbn isbn,
                    int publicationYear,
                    string? description,
                    Guid authorId,
                    Guid categoryId)
        {
            ApplyTitleRules(title);
            ApplyIsbnRules(isbn);
            ApplyPublicationYearRules(publicationYear);
            ApplyDescriptionRules(description);
            ApplyAuthorIdRules(authorId);
            ApplyCategoryIdRules(categoryId);

            Id = Guid.CreateVersion7();
            Title = title.Trim();
            Isbn = isbn;
            PublicationYear = publicationYear;
            Description = description?.Trim();
            AuthorId = authorId;
            CategoryId = categoryId;
        }

        private static void ApplyTitleRules(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new BusinessRuleException("El título es requerido.");
            }

            if (title.Trim().Length > 256)
            {
                throw new BusinessRuleException("El título debe tener máximo 256 caracteres.");
            }
        }

        private static void ApplyIsbnRules(Isbn isbn)
        {
            if (isbn is null)
            {
                throw new BusinessRuleException("El ISBN es requerido.");
            }
        }

        private static void ApplyPublicationYearRules(int publicationYear)
        {
            int currentYear = DateTime.UtcNow.Year;

            if (publicationYear < MIN_PUBLICATION_YEAR || publicationYear > currentYear)
            {
                throw new BusinessRuleException($"El año de publicación debe estar entre {MIN_PUBLICATION_YEAR} y {currentYear}.");
            }
        }

        private static void ApplyDescriptionRules(string? description)
        {
            if (!string.IsNullOrWhiteSpace(description) && description.Trim().Length > 2048)
            {
                throw new BusinessRuleException("La descripción del libro debe tener máximo 2048 caracteres.");
            }
        }

        private static void ApplyAuthorIdRules(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new BusinessRuleException("El autor es requerido.");
            }
        }

        private static void ApplyCategoryIdRules(Guid categoryId)
        {
            if (categoryId == Guid.Empty)
            {
                throw new BusinessRuleException("La categoría es requerida.");
            }
        }
    }
}
