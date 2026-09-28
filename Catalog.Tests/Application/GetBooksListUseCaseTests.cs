using Catalog.Application.UseCases.Books.Common;
using Catalog.Application.UseCases.Books.Queries.GetBooksList;
using Catalog.Application.Utilities.Pagination;
using Catalog.Domain.Entities.Authors;
using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Categories;

namespace Catalog.Tests.Application
{
    [TestClass]
    public sealed class GetBooksListUseCaseTests
    {
        private static GetBooksListUseCase CreateUseCase(int totalBooks)
        {
            Author author = TestData.CreateAuthor();
            Category category = TestData.CreateCategory();

            // ISBN válidos generados a partir de un prefijo para no repetirlos
            string[] isbns =
            [
                "9780307474728", "9780307387264", "9780451524935", "9780441172719", "9780553293357",
            ];

            List<Book> books = Enumerable.Range(0, totalBooks)
                                         .Select(i => TestData.CreateBook($"Libro {i:00}", isbns[i], 2000 + i, author, category))
                                         .ToList();

            return new GetBooksListUseCase(new FakeBooksRepository(books));
        }

        [TestMethod]
        public async Task Handle_ReturnsMappedItems()
        {
            GetBooksListUseCase useCase = CreateUseCase(totalBooks: 3);

            PaginationResponse<BookListItemDTO> result = await useCase.Handle(new GetBooksListQuery());

            Assert.AreEqual(3, result.TotalCount);
            Assert.AreEqual(3, result.Items.Count);

            BookListItemDTO first = result.Items[0];
            Assert.AreEqual("Libro 00", first.Title);
            Assert.AreEqual("9780307474728", first.Isbn);
            Assert.AreEqual(2000, first.PublicationYear);
            Assert.AreEqual("Gabriel García Márquez", first.AuthorName);
            Assert.AreEqual("Novela", first.CategoryName);
        }

        [TestMethod]
        public async Task Handle_AppliesPagination()
        {
            GetBooksListUseCase useCase = CreateUseCase(totalBooks: 5);

            GetBooksListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber: 2, pageSize: 2)
            };

            PaginationResponse<BookListItemDTO> result = await useCase.Handle(query);

            Assert.AreEqual(5, result.TotalCount);
            Assert.AreEqual(2, result.Items.Count);
            Assert.AreEqual(3, result.TotalPages);
            Assert.AreEqual("Libro 02", result.Items[0].Title);
            Assert.IsTrue(result.HasPreviousPage);
            Assert.IsTrue(result.HasNextPage);
        }
    }
}
