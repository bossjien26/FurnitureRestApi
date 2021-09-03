using DbEntity;
using Entities;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class ProductSpecificationRepository : GenericRepository<ProductSpecification> , IProductSpecificationRepository
    {
        public ProductSpecificationRepository(DbContextEntity context) : base(context)
        {

        }
    }
}