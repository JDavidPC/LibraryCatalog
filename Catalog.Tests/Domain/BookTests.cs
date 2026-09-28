using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Books.ValueObjects;
using Catalog.Domain.Exceptions;

namespace Catalog.Tests.Domain
{
    [TestClass]
    public sealed class BookTests
    {
        private static readonly Isbn ValidIsbn = new("9780307474728");

        [TestMethod]
        public void Create_WithValidData_SetsProperties()
        {
            Guid authorId = Guid.NewGuid();
            Guid categoryId = Guid.NewGuid();

            Book book = new("  Cien años de soledad  ", ValidIsbn, 1967, "Descripción", authorId, categoryId);

            Assert.AreNotEqual(Guid.Empty, book.Id);
            Assert.AreEqual("Cien años de soledad", book.Title);
            Assert.AreEqual(ValidIsbn, book.Isbn);
            Assert.AreEqual(1967, book.PublicationYear);
            Assert.AreEqual(authorId, book.AuthorId);
            Assert.AreEqual(categoryId, book.CategoryId);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("   ")]
        public void Create_WithEmptyTitle_ThrowsBusinessRuleException(string title)
        {
            Assert.ThrowsExactly<BusinessRuleException>(
                () => { _ = new Book(title, ValidIsbn, 1967, null, Guid.NewGuid(), Guid.NewGuid()); });
        }

        [TestMethod]
        public void Create_WithFutureYear_ThrowsBusinessRuleException()
        {
            int nextYear = DateTime.UtcNow.Year + 1;

            Assert.ThrowsExactly<BusinessRuleException>(
                () => { _ = new Book("Título", ValidIsbn, nextYear, null, Guid.NewGuid(), Guid.NewGuid()); });
        }

        [TestMethod]
        public void Create_WithYearBeforeMinimum_ThrowsBusinessRuleException()
        {
            Assert.ThrowsExactly<BusinessRuleException>(
                () => { _ = new Book("Título", ValidIsbn, Book.MIN_PUBLICATION_YEAR - 1, null, Guid.NewGuid(), Guid.NewGuid()); });
        }

        [TestMethod]
        public void Create_WithEmptyAuthorId_ThrowsBusinessRuleException()
        {
            Assert.ThrowsExactly<BusinessRuleException>(
                () => { _ = new Book("Título", ValidIsbn, 1967, null, Guid.Empty, Guid.NewGuid()); });
        }

        [TestMethod]
        public void Create_WithEmptyCategoryId_ThrowsBusinessRuleException()
        {
            Assert.ThrowsExactly<BusinessRuleException>(
                () => { _ = new Book("Título", ValidIsbn, 1967, null, Guid.NewGuid(), Guid.Empty); });
        }
    }
}
