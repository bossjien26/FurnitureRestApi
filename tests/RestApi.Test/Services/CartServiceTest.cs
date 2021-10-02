using System.Threading.Tasks;
using Entities;
using Enum;
using NUnit.Framework;
using RestApi.Test.Repositories;
using Services.Interface.Redis;
using Services.Redis;
using StackExchange.Redis;

namespace RestApi.Test.Services
{
    [TestFixture]
    public class CartServiceTest : BaseRepositoryTest
    {
        private readonly ICartService _cartService;

        public CartServiceTest()
        {
            _cartService = new CartService(_redisConnect);
        }

        [Test]
        public void ShouldSet()
        {
            var entity = new Cart();
            entity.UserId = "1";
            entity.ProductId = "1";
            entity.Quantity = "1";
            entity.Attribute = CartAttributeEnum.Shopping;
            Assert.DoesNotThrowAsync(() => _cartService.Set(entity));
        }

        [Test]
        public async Task ShouldGetById()
        {
            var result = await _cartService.GetById("1", "1", CartAttributeEnum.Shopping);

            Assert.IsInstanceOf<RedisValue>(result);
        }

        [Test]
        public void ShouldDelete()
        {
            Assert.DoesNotThrow(() =>
                _cartService.Delete("1", "1", CartAttributeEnum.Shopping)
            );
        }

        [Test]
        public void ShouldGetMany()
        {
            var db = _redisConnect.GetDatabase();
            var result = _cartService.GetMany("1", CartAttributeEnum.Shopping);
            Assert.IsInstanceOf<HashEntry[]>(result);
            Assert.AreEqual(db.HashLength("cart:1"), result.Length);
        }
    }
}