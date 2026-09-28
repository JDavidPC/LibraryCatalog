using Catalog.Application;
using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Exceptions;
using Catalog.Application.UseCases.Books.Queries.GetBookById;
using Catalog.Application.Utilities.Mediator;
using Catalog.Domain.Entities.Authors;
using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Categories;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Tests.Application
{
    [TestClass]
    public sealed class SimpleMediatorTests
    {
        private static IMediator CreateMediator(IEnumerable<Book> books)
        {
            ServiceCollection services = new();

            services.AddApplicationServices();
            services.AddSingleton<IBooksRepository>(new FakeBooksRepository(books));
            services.AddSingleton<ICategoriesRepository>(new FakeCategoriesRepository([]));

            return services.BuildServiceProvider().GetRequiredService<IMediator>();
        }

        [TestMethod]
        public async Task Send_WithRegisteredHandler_ReturnsResponse()
        {
            Book book = TestData.CreateBook("Cien años de soledad",
                                            "9780307474728",
                                            1967,
                                            TestData.CreateAuthor(),
                                            TestData.CreateCategory());

            IMediator mediator = CreateMediator([book]);

            BookDetailDTO result = await mediator.Send(new GetBookByIdQuery { Id = book.Id });

            Assert.AreEqual(book.Id, result.Id);
        }

        [TestMethod]
        public async Task Send_WhenHandlerThrows_PropagatesOriginalException()
        {
            IMediator mediator = CreateMediator([]);

            // Debe llegar NotFoundException y no un TargetInvocationException
            await Assert.ThrowsExactlyAsync<NotFoundException>(
                () => mediator.Send(new GetBookByIdQuery { Id = Guid.NewGuid() }));
        }

        [TestMethod]
        public async Task Send_WithoutRegisteredHandler_ThrowsMediatorException()
        {
            IMediator mediator = new SimpleMediator(new ServiceCollection().BuildServiceProvider());

            await Assert.ThrowsExactlyAsync<MediatorException>(
                () => mediator.Send(new GetBookByIdQuery { Id = Guid.NewGuid() }));
        }
    }
}
