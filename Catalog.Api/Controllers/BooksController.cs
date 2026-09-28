using Catalog.Application.UseCases.Books.Common;
using Catalog.Application.UseCases.Books.Queries.GetBookById;
using Catalog.Application.UseCases.Books.Queries.GetBooksByCategory;
using Catalog.Application.UseCases.Books.Queries.GetBooksList;
using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Query 1 - Consulta el listado de libros registrados.
        /// </summary>
        [HttpGet]
        [ProducesResponseType<PaginationResponse<BookListItemDTO>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetList([FromQuery] int pageNumber = 1,
                                                 [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE)
        {
            GetBooksListQuery query = new()
            {
                Pagination = new PaginationRequest(pageNumber, pageSize)
            };

            PaginationResponse<BookListItemDTO> result = await _mediator.Send(query);

            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Query 2 - Consulta el detalle de un libro por su Id.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType<BookDetailDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            GetBookByIdQuery query = new()
            {
                Id = id
            };

            BookDetailDTO result = await _mediator.Send(query);

            return StatusCode(StatusCodes.Status200OK, result);
        }

        /// <summary>
        /// Query 3 - Consulta los libros pertenecientes a una categoría.
        /// </summary>
        [HttpGet("category/{categoryId:guid}")]
        [ProducesResponseType<PaginationResponse<BookListItemDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetByCategory([FromRoute] Guid categoryId,
                                                       [FromQuery] int pageNumber = 1,
                                                       [FromQuery] int pageSize = PaginationRequest.DEFAULT_PAGE_SIZE)
        {
            GetBooksByCategoryQuery query = new()
            {
                CategoryId = categoryId,
                Pagination = new PaginationRequest(pageNumber, pageSize)
            };

            PaginationResponse<BookListItemDTO> result = await _mediator.Send(query);

            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}
