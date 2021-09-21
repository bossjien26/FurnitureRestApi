using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContextEntity context) : base(context)
        {

        }
    }
}