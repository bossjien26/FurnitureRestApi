using System.Threading.Tasks;
using Entities;
using StackExchange.Redis;

namespace Services.IService.Redis
{
    public interface ICartService
    {
        Task<bool> Set(Cart instance);

        Task<RedisValue> GetById(string HashId, string key);

        HashEntry[] GetMany(string HashId);

        bool Delete(string HashId,string key);
    }
}