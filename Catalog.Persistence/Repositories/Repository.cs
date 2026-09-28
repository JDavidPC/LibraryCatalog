using Catalog.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DataContext _context;

        public Repository(DataContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                                 .AsNoTracking()
                                 .ToListAsync(cancellationToken);
        }
    }
}
