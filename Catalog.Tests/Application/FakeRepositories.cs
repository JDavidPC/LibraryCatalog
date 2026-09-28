using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Pagination;
using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Categories;

namespace Catalog.Tests.Application
{
    internal class FakeBooksRepository : IBooksRepository
    {
        private readonly List<Book> _books;

        public FakeBooksRepository(IEnumerable<Book> books)
        {
            _books = books.ToList();
        }

        public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_books.FirstOrDefault(b => b.Id == id));
        }

        public Task<IEnumerable<Book>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Book>>(_books);
        }

        public Task<Book?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_books.FirstOrDefault(b => b.Id == id));
        }

        public Task<PaginationResponse<Book>> GetPagedListAsync(PaginationRequest request,
                                                                CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Page(_books, request));
        }

        public Task<PaginationResponse<Book>> GetPagedListByCategoryAsync(PaginationRequest request,
                                                                          Guid categoryId,
                                                                          CancellationToken cancellationToken = default)
        {
            List<Book> filtered = _books.Where(b => b.CategoryId == categoryId).ToList();

            return Task.FromResult(Page(filtered, request));
        }

        private static PaginationResponse<Book> Page(List<Book> source, PaginationRequest request)
        {
            List<Book> items = source.OrderBy(b => b.Title)
                                     .Skip((request.PageNumber - 1) * request.PageSize)
                                     .Take(request.PageSize)
                                     .ToList();

            return PaginationResponse<Book>.Create(items, source.Count, request);
        }
    }

    internal class FakeCategoriesRepository : ICategoriesRepository
    {
        private readonly List<Category> _categories;

        public FakeCategoriesRepository(IEnumerable<Category> categories)
        {
            _categories = categories.ToList();
        }

        public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_categories.FirstOrDefault(c => c.Id == id));
        }

        public Task<IEnumerable<Category>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IEnumerable<Category>>(_categories);
        }
    }
}
