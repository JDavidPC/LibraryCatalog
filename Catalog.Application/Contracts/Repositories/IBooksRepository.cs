using Catalog.Application.Utilities.Pagination;
using Catalog.Domain.Entities.Books;

namespace Catalog.Application.Contracts.Repositories
{
    public interface IBooksRepository : IRepository<Book>
    {
        /// <summary>
        /// Retorna el libro con su autor y su categoría cargados.
        /// </summary>
        Task<Book?> GetDetailByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<PaginationResponse<Book>> GetPagedListAsync(PaginationRequest request,
                                                         CancellationToken cancellationToken = default);

        Task<PaginationResponse<Book>> GetPagedListByCategoryAsync(PaginationRequest request,
                                                                   Guid categoryId,
                                                                   CancellationToken cancellationToken = default);
    }
}
