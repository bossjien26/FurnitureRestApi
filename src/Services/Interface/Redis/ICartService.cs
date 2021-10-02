using System.Threading.Tasks;
using Entities;
using Enum;
using StackExchange.Redis;

namespace Services.Interface.Redis
{
    public interface ICartService
    {
        Task Set(Cart instance);

        Task<RedisValue> GetById(string HashId, string key,CartAttributeEnum cartAttribute);

        HashEntry[] GetMany(string HashId,CartAttributeEnum cartAttribute);

        bool Delete(string HashId,string key,CartAttributeEnum cartAttribute);
    }
}