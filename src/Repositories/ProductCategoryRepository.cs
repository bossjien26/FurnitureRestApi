using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class ProductCategoryRepository: GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(DbContextEntity context) : base(context)
        {

        }
        
    }
}