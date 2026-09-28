using Catalog.Application.Contracts.Repositories;
using Catalog.Application.UseCases.Books.Common;
using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;
using Catalog.Domain.Entities.Books;

namespace Catalog.Application.UseCases.Books.Queries.GetBooksList
{
    public class GetBooksListUseCase : IRequestHandler<GetBooksListQuery, PaginationResponse<BookListItemDTO>>
    {
        private readonly IBooksRepository _repository;

        public GetBooksListUseCase(IBooksRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginationResponse<BookListItemDTO>> Handle(GetBooksListQuery query)
        {
            PaginationRequest pagination = query.Pagination;

            PaginationResponse<Book> response = await _repository.GetPagedListAsync(pagination);

            List<BookListItemDTO> items = response.Items.Select(b => b.ToListItemDTO())
                                                        .ToList();

            return PaginationResponse<BookListItemDTO>.Create(items, response.TotalCount, pagination);
        }
    }
}
