using System.Threading.Tasks;
using Entities;
using Enum;
using StackExchange.Redis;

namespace Services.Interface.Redis
{
    public interface ICartService
    {
        Task<bool> Set(Cart instance);

        Task<RedisValue> GetById(string HashId, string key,CartAttribute cartAttribute);

        HashEntry[] GetMany(string HashId,CartAttribute cartAttribute);

        bool Delete(string HashId,string key,CartAttribute cartAttribute);
    }
}