using Catalog.Application.Exceptions;
using Catalog.Application.UseCases.Books.Queries.GetBookById;
using Catalog.Domain.Entities.Authors;
using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Categories;

namespace Catalog.Tests.Application
{
    [TestClass]
    public sealed class GetBookByIdUseCaseTests
    {
        [TestMethod]
        public async Task Handle_WithExistingId_ReturnsDetailWithAuthorAndCategory()
        {
            Author author = TestData.CreateAuthor();
            Category category = TestData.CreateCategory();
            Book book = TestData.CreateBook("Cien años de soledad", "9780307474728", 1967, author, category);

            GetBookByIdUseCase useCase = new(new FakeBooksRepository([book]));

            BookDetailDTO result = await useCase.Handle(new GetBookByIdQuery { Id = book.Id });

            Assert.AreEqual(book.Id, result.Id);
            Assert.AreEqual("Cien años de soledad", result.Title);
            Assert.AreEqual("9780307474728", result.Isbn);
            Assert.AreEqual(1967, result.PublicationYear);
            Assert.AreEqual("Descripción del libro", result.Description);
            Assert.AreEqual(author.Id, result.AuthorId);
            Assert.AreEqual("Gabriel García Márquez", result.AuthorName);
            Assert.AreEqual("Colombiana", result.AuthorNationality);
            Assert.AreEqual(category.Id, result.CategoryId);
            Assert.AreEqual("Novela", result.CategoryName);
        }

        [TestMethod]
        public async Task Handle_WithUnknownId_ThrowsNotFoundException()
        {
            GetBookByIdUseCase useCase = new(new FakeBooksRepository([]));

            await Assert.ThrowsExactlyAsync<NotFoundException>(
                () => useCase.Handle(new GetBookByIdQuery { Id = Guid.NewGuid() }));
        }
    }
}
