using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.Interface;
using Repositories;
using Services.Interface;
using StackExchange.Redis;
using System;
using Enum;
using Services.Dto;

namespace Services
{
    public class UserService : IUserService
    {
        private IUserRepository _repository;

        private readonly IDatabase _redisDb;

        public UserService(DbContextEntity dbContextEntity, IConnectionMultiplexer redis)
        {
            _repository = new UserRepository(dbContextEntity);
            _redisDb = redis.GetDatabase();
        }

        public UserService(IUserRepository genericRepository)
                => _repository = genericRepository;

        public async Task Insert(User instance) => await _repository.Insert(instance);

        public async Task Update(User instance) => await _repository.Update(instance);

        public IQueryable<User> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<User> GetMany(int index, int size)
        {
            return _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);
        }

        public async Task<User> GetById(int userId)
        {
            return await _repository.Get(c => c.Id == userId);
        }

        public async Task<User> GetVerifyUser(string mail, string password)
        {
            return await _repository.Get(user => user.Mail == mail
            && user.Password == password);
        }

        public async Task<User> SearchUserMail(string mail)
        {
            return await _repository.Get(user => user.Mail == mail);
        }

        public async Task Login(string token, string userId)
        => await _redisDb.StringSetAsync(token, userId.ToString());

        public async Task UserExpireDateTime(string token, DateTime dateTime)
        => await _redisDb.KeyExpireAsync(token, dateTime);

        public async Task Logout(string token)
        => await _redisDb.KeyDeleteAsync(token);

        public async Task<RedisValue> GetRedisUserInfo(string token)
        => await _redisDb.StringGetAsync(token);

        public UserInfo MapShowUserInfo(User user)
        => new UserInfo()
        {
            Name = user.Name,
            Mail = user.Mail,
            Role = user.Role,
            RoleName = GetUserRole(user.Role)
        };

        private string GetUserRole(RoleEnum role)
        {
            switch (role)
            {
                case RoleEnum.SuperAdmin:
                    return "SuperAdmin";
                case RoleEnum.Admin:
                    return "Admin";
                case RoleEnum.Staff:
                    return "Staff";
                case RoleEnum.Customer:
                    return "Customer";
                default:
                    return "";
            }
        }
    }
}