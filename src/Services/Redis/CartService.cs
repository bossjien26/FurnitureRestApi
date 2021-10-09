using System.Threading.Tasks;
using Entities;
using Enum;
using Services.Interface.Redis;
using StackExchange.Redis;

namespace Services.Redis
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

        private string hashIdType(string hashId, CartAttributeEnum cartAttribute)
        => CartAttributeEnum.Shopping == cartAttribute ?
            "cart:" + hashId : "likelist:" + hashId;

        private HashEntry[] hashSetValue(string key, string value) => new HashEntry[] { new HashEntry(key, value) };

        public async Task Set(Cart instance)
        => await _db.HashSetAsync(hashIdType(instance.UserId, instance.Attribute), hashSetValue(instance.InventoryId, instance.Quantity));

        public async Task<RedisValue> GetById(string HashId, string key, CartAttributeEnum cartAttribute)
        => await _db.HashGetAsync(hashIdType(HashId, cartAttribute), key);

        public HashEntry[] GetMany(string HashId, CartAttributeEnum cartAttribute)
        => _db.HashGetAll(hashIdType(HashId, cartAttribute));

        public bool Delete(string HashId, string key, CartAttributeEnum cartAttribute)
        => _db.HashDelete(hashIdType(HashId, cartAttribute), key);
    }
}