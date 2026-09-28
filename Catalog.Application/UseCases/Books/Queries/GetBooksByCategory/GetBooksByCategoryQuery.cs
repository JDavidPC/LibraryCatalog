using Catalog.Application.UseCases.Books.Common;
using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;

namespace Catalog.Application.UseCases.Books.Queries.GetBooksByCategory
{
    public class GetBooksByCategoryQuery : IRequest<PaginationResponse<BookListItemDTO>>
    {
        public Guid CategoryId { get; set; }
        public PaginationRequest Pagination { get; set; } = PaginationRequest.Standard();
    }
}
