using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.IRepository;
using Repositories.Repository;
using Services.IService;

namespace Services.Service
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;

        public UserService(DbContextEntity dbContextEntity)
        {
            _repository = new UserRepository(dbContextEntity);
        }

        public UserService(IUserRepository genericRepository)
                => _repository = genericRepository;

        public async Task Insert(User instance) => await _repository.Insert(instance);

        public void Update(User instance) => _repository.Update(instance);

        public IEnumerable<User> GetAllUser()
        {
            return _repository.GetAll();
        }

        public IEnumerable<User> GetMany(int index, int size)
        {
            return _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size);
        }

        public async Task<User> GetById(int userId)
        {
            return await _repository.GetById(c => c.Id == userId);
        }

        public User GetVerifyUser(string mail, string password)
        {
            return _repository.GetAll().Where(user => user.Mail == mail
            && user.Password == password).OrderByDescending(x => x.Id).Take(1).FirstOrDefault();
        }

        public User SearchUserMail(string mail)
        {
            return _repository.GetAll().Where(User => User.Mail == mail).Take(1).OrderByDescending(x => x.Id).FirstOrDefault();
        }
    }
}