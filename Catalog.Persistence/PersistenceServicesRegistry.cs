using Catalog.Application.Contracts.Repositories;
using Catalog.Persistence.Repositories;
using Catalog.Persistence.Seeds;
using Catalog.Persistence.Seeds.Authors;
using Catalog.Persistence.Seeds.Books;
using Catalog.Persistence.Seeds.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.Persistence
{
    public static class PersistenceServicesRegistry
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("MyConnection"));
            });

            services.AddScoped<IBooksRepository, BooksRepository>();
            services.AddScoped<ICategoriesRepository, CategoriesRepository>();

            // Seeders
            services.AddScoped<IDataSeeder, CategorySeeder>();
            services.AddScoped<IDataSeeder, AuthorSeeder>();
            services.AddScoped<IDataSeeder, BookSeeder>();

            return services;
        }
    }
}
