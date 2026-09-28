using Catalog.Application.UseCases.Books.Common;
using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;

namespace Catalog.Application.UseCases.Books.Queries.GetBooksList
{
    public class GetBooksListQuery : IRequest<PaginationResponse<BookListItemDTO>>
    {
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standard();
    }
}
