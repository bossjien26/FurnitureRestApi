using DbEntity;
using Entities;
using Repositories.IRepository;
using src.Repositories.Repository;

namespace Repositories.Repository
{
    public class UserDetailRepository : GenericRepository<UserDetail> , IUserDetailRepository
    {
        public UserDetailRepository(DbContextEntity context):base(context)
        {
            
        }
    }
}