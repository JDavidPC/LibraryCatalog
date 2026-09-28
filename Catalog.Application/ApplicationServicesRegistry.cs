using Catalog.Application.UseCases.Books.Common;
using Catalog.Application.UseCases.Books.Queries.GetBookById;
using Catalog.Application.UseCases.Books.Queries.GetBooksByCategory;
using Catalog.Application.UseCases.Books.Queries.GetBooksList;
using Catalog.Application.Utilities.Mediator;
using Catalog.Application.Utilities.Pagination;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Application
{
    public static class ApplicationServicesRegistry
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Mediator
            services.AddScoped<IMediator, SimpleMediator>();

            // Use Cases
            services.AddScoped<IRequestHandler<GetBooksListQuery, PaginationResponse<BookListItemDTO>>, GetBooksListUseCase>();
            services.AddScoped<IRequestHandler<GetBookByIdQuery, BookDetailDTO>, GetBookByIdUseCase>();
            services.AddScoped<IRequestHandler<GetBooksByCategoryQuery, PaginationResponse<BookListItemDTO>>, GetBooksByCategoryUseCase>();

            return services;
        }
    }
}
