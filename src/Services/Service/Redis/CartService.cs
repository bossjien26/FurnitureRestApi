using System.Threading.Tasks;
using Entities;
using Services.IService.Redis;
using StackExchange.Redis;

namespace Services.Service.Redis
{
    public class CartService : ICartService
    {
        private readonly IConnectionMultiplexer _redis;

        private readonly IDatabase _db;

        public CartService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public async Task<bool> Set(Cart instance)
        {
            return await _db.HashSetAsync(instance.UserId,instance.ProductId,instance.Quantity);
        }

        public async Task<RedisValue> GetById(string HashId,string key)
        {
            return await _db.HashGetAsync(HashId,key);
        }

        public HashEntry[] GetMany(string HashId)
        {
            return _db.HashGetAll(HashId);
        }

        public bool Delete(string HashId,string key)
        {
            return _db.HashDelete(HashId,key);
        }
    }
}