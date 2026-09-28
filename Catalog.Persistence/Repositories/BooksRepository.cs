using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Utilities.Pagination;
using Catalog.Domain.Entities.Books;
using Catalog.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Repositories
{
    public class BooksRepository : Repository<Book>, IBooksRepository
    {
        public BooksRepository(DataContext context) : base(context)
        {
        }

        public async Task<Book?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Book>()
                                 .AsNoTracking()
                                 .Include(b => b.Author)
                                 .Include(b => b.Category)
                                 .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<PaginationResponse<Book>> GetPagedListAsync(PaginationRequest request,
                                                                      CancellationToken cancellationToken = default)
        {
            IQueryable<Book> query = _context.Set<Book>()
                                             .AsNoTracking()
                                             .Include(b => b.Author)
                                             .Include(b => b.Category)
                                             .AsQueryable();

            query = query.OrderBy(b => b.Title)
                         .ThenBy(b => b.Id);

            return await query.ToPagedListAsync(request, cancellationToken);
        }

        public async Task<PaginationResponse<Book>> GetPagedListByCategoryAsync(PaginationRequest request,
                                                                                Guid categoryId,
                                                                                CancellationToken cancellationToken = default)
        {
            IQueryable<Book> query = _context.Set<Book>()
                                             .AsNoTracking()
                                             .Include(b => b.Author)
                                             .Include(b => b.Category)
                                             .Where(b => b.CategoryId == categoryId)
                                             .AsQueryable();

            query = query.OrderBy(b => b.Title)
                         .ThenBy(b => b.Id);

            return await query.ToPagedListAsync(request, cancellationToken);
        }
    }
}
