using Catalog.Application.Utilities.Mediator;

namespace Catalog.Application.UseCases.Books.Queries.GetBookById
{
    public class GetBookByIdQuery : IRequest<BookDetailDTO>
    {
        public Guid Id { get; set; }
    }
}
