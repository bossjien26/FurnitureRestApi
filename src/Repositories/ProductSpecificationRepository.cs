using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class ProductSpecificationRepository : GenericRepository<ProductSpecification> , IProductSpecificationRepository
    {
        public ProductSpecificationRepository(DbContextEntity context) : base(context)
        {

        }
    }
}