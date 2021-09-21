using DbEntity;
using Entities;
using Repositories.Interface;

namespace Repositories
{
    public class UserDetailRepository : GenericRepository<UserDetail> , IUserDetailRepository
    {
        public UserDetailRepository(DbContextEntity context):base(context)
        {
            
        }
    }
}