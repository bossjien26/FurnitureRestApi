using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.Interface;
using Repositories;
using Services.Interface;
using StackExchange.Redis;
using System;

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

        public void Update(User instance) => _repository.Update(instance);

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

        public User GetVerifyUser(string mail, string password)
        {
            return _repository.GetAll().Where(user => user.Mail == mail
            && user.Password == password).OrderByDescending(x => x.Id).Take(1).FirstOrDefault();
        }

        public User SearchUserMail(string mail)
        {
            return _repository.GetAll().Where(User => User.Mail == mail).Take(1).OrderByDescending(x => x.Id).FirstOrDefault();
        }

        public async Task Login(string token, string userId)
        => await _redisDb.StringSetAsync(token, userId.ToString());

        public async Task UserExpireDateTime(string token, DateTime dateTime)
        => await _redisDb.KeyExpireAsync(token, dateTime);

        public async Task Logout(string token)
        => await _redisDb.KeyDeleteAsync(token);

        public async Task<RedisValue> GetRedisUserId(string token)
        => await _redisDb.StringGetAsync(token);
    }
}