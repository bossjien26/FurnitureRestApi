using DbEntity;
using Entities;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(DbContextEntity context) : base(context)
        {

        }
    }
}