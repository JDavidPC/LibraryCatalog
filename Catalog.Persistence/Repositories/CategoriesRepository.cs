using Catalog.Application.Contracts.Repositories;
using Catalog.Domain.Entities.Categories;

namespace Catalog.Persistence.Repositories
{
    public class CategoriesRepository : Repository<Category>, ICategoriesRepository
    {
        public CategoriesRepository(DataContext context) : base(context)
        {
        }
    }
}
