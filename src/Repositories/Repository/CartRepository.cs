using DbEntity;
using Entities;
using Repositories.IRepository;

namespace Repositories.Repository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(DbContextEntity context) : base(context)
        {

        }
    }
}