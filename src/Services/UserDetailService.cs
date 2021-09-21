using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.IRepository;
using Repositories.Repository;
using Services.Interface;

namespace Services
{
    public class UserDetailService : IUserDetailService
    {
        private IUserDetailRepository _repository;

        public UserDetailService(DbContextEntity dbContextEntity)
        {
            _repository = new UserDetailRepository(dbContextEntity);
        }

        public UserDetailService(IUserDetailRepository genericRepository)
                => _repository = genericRepository;

        public async Task Insert(UserDetail instance) => await _repository.Insert(instance);
    }
}