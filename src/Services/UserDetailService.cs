using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.Interface;
using Repositories;
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

        public async Task Update(UserDetail instance) => await _repository.Update(instance);
 
        public async Task<UserDetail> GetUserInfo(int id) => await _repository.Get(x => x.Id == id);
    }
}