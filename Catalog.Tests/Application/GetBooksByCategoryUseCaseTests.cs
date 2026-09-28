using Catalog.Application.Exceptions;
using Catalog.Application.UseCases.Books.Common;
using Catalog.Application.UseCases.Books.Queries.GetBooksByCategory;
using Catalog.Application.Utilities.Pagination;
using Catalog.Domain.Entities.Authors;
using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Categories;

namespace Catalog.Tests.Application
{
    [TestClass]
    public sealed class GetBooksByCategoryUseCaseTests
    {
        private readonly Author _author = TestData.CreateAuthor();
        private readonly Category _novel = TestData.CreateCategory("Novela");
        private readonly Category _history = TestData.CreateCategory("Historia");

        private GetBooksByCategoryUseCase CreateUseCase()
        {
            List<Book> books =
            [
                TestData.CreateBook("Cien años de soledad", "9780307474728", 1967, _author, _novel),
                TestData.CreateBook("El amor en los tiempos del cólera", "9780307387264", 1985, _author, _novel),
                TestData.CreateBook("Sapiens", "9780062316097", 2014, _author, _history),
            ];

            return new GetBooksByCategoryUseCase(new FakeBooksRepository(books),
                                                 new FakeCategoriesRepository([_novel, _history]));
        }

        [TestMethod]
        public async Task Handle_WithExistingCategory_ReturnsOnlyBooksOfThatCategory()
        {
            GetBooksByCategoryUseCase useCase = CreateUseCase();

            PaginationResponse<BookListItemDTO> result = await useCase.Handle(new GetBooksByCategoryQuery { CategoryId = _novel.Id });

            Assert.AreEqual(2, result.TotalCount);
            Assert.IsTrue(result.Items.All(b => b.CategoryId == _novel.Id));
            Assert.IsTrue(result.Items.All(b => b.CategoryName == "Novela"));
        }

        [TestMethod]
        public async Task Handle_WithCategoryWithoutBooks_ReturnsEmptyList()
        {
            Category empty = TestData.CreateCategory("Poesía");

            GetBooksByCategoryUseCase useCase = new(new FakeBooksRepository([]),
                                                    new FakeCategoriesRepository([empty]));

            PaginationResponse<BookListItemDTO> result = await useCase.Handle(new GetBooksByCategoryQuery { CategoryId = empty.Id });

            Assert.AreEqual(0, result.TotalCount);
            Assert.AreEqual(0, result.Items.Count);
        }

        [TestMethod]
        public async Task Handle_WithUnknownCategory_ThrowsNotFoundException()
        {
            GetBooksByCategoryUseCase useCase = CreateUseCase();

            await Assert.ThrowsExactlyAsync<NotFoundException>(
                () => useCase.Handle(new GetBooksByCategoryQuery { CategoryId = Guid.NewGuid() }));
        }
    }
}
