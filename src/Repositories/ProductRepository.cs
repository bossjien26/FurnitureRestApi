using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContextEntity context) : base(context)
        {

        }
    }
}