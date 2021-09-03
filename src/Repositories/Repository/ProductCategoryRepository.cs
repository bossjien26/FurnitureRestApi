using DbEntity;
using Entities;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class ProductCategoryRepository: GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(DbContextEntity context) : base(context)
        {

        }
        
    }
}