using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Exceptions;
using Catalog.Application.UseCases.Books.Common;
using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;
using Catalog.Domain.Entities.Books;
using Catalog.Domain.Entities.Categories;

namespace Catalog.Application.UseCases.Books.Queries.GetBooksByCategory
{
    public class GetBooksByCategoryUseCase : IRequestHandler<GetBooksByCategoryQuery, PaginationResponse<BookListItemDTO>>
    {
        private readonly IBooksRepository _booksRepository;
        private readonly ICategoriesRepository _categoriesRepository;

        public GetBooksByCategoryUseCase(IBooksRepository booksRepository,
                                         ICategoriesRepository categoriesRepository)
        {
            _booksRepository = booksRepository;
            _categoriesRepository = categoriesRepository;
        }

        public async Task<PaginationResponse<BookListItemDTO>> Handle(GetBooksByCategoryQuery query)
        {
            Category? category = await _categoriesRepository.GetByIdAsync(query.CategoryId);

            if (category is null)
            {
                throw new NotFoundException($"No se encontró la categoría con Id {query.CategoryId}.");
            }

            PaginationRequest pagination = query.Pagination;

            PaginationResponse<Book> response = await _booksRepository.GetPagedListByCategoryAsync(pagination,
                                                                                                    query.CategoryId);

            List<BookListItemDTO> items = response.Items.Select(b => b.ToListItemDTO())
                                                        .ToList();

            return PaginationResponse<BookListItemDTO>.Create(items, response.TotalCount, pagination);
        }
    }
}
