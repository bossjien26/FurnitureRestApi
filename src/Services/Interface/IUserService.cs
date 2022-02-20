using System;
using System.Linq;
using System.Threading.Tasks;
using Entities;
using Services.Dto;
using StackExchange.Redis;

namespace Services.Interface
{
    public interface IUserService
    {
        Task Insert(User instance);

        Task Update(User instance);

        IQueryable<User> GetAll();

        IQueryable<User> GetMany(int index, int size);

        Task<User> GetById(int userId);

        Task<User> GetVerifyUser(string mail, string password);

        Task<User> SearchUserMail(string mail);

        Task Login(string token, string userId);

        Task UserExpireDateTime(string token, DateTime dateTime);

        Task Logout(string token);

        Task<RedisValue> GetRedisUserInfo(string token);

        UserInfo MapShowUserInfo(User user);
    }
}