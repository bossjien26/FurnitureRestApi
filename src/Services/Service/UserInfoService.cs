using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using src.Entities;
using src.Repositories.IRepository;
using src.Repositories.Repository;
using src.Services.IService;

namespace src.Services.Service
{
    public class UserInfoService : IUserInfoService
    {
        private IUserRepository _repository;

        public UserInfoService(DbContextEntity dbContextEntity)
        {
            _repository = new UserRepository(dbContextEntity);
        }

        public void Insert(User instance) => _repository.Insert(instance);

        public void Update(User instance) => _repository.Update(instance);

        public UserInfoService(IUserRepository genericRepository) 
                => _repository = genericRepository;

        public IEnumerable<User> GetAllUser()
        {
            return _repository.GetAll();
        }

        public async Task<User> FindUser(int userId)
        {
            return  await _repository.GetById(c => c.Id == userId);
        }
    }
}