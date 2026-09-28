using Catalog.Application.Contracts.Repositories;
using Catalog.Application.Exceptions;
using Catalog.Application.Utilities.Mediator;
using Catalog.Domain.Entities.Books;

namespace Catalog.Application.UseCases.Books.Queries.GetBookById
{
    public class GetBookByIdUseCase : IRequestHandler<GetBookByIdQuery, BookDetailDTO>
    {
        private readonly IBooksRepository _repository;

        public GetBookByIdUseCase(IBooksRepository repository)
        {
            _repository = repository;
        }

        public async Task<BookDetailDTO> Handle(GetBookByIdQuery query)
        {
            Book? book = await _repository.GetDetailByIdAsync(query.Id);

            if (book is null)
            {
                throw new NotFoundException($"No se encontró el libro con Id {query.Id}.");
            }

            return book.ToDetailDTO();
        }
    }
}
