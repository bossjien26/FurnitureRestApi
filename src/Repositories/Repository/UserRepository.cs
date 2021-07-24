using DbEntity;
using src.Entities;
using src.Repositories.IRepository;

namespace src.Repositories.Repository
{
    public class UserRepository : GenericRepository<User> , IUserRepository
    {
        public UserRepository(DbContextEntity context):base(context)
        {
            
        }
    }
}