using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using Entities;
using Enum;

namespace RestApi.Test.DatabaseSeeders
{
    public class CartSeeder
    {
        public static Cart SeedOne() => SetCart().Generate();

        public static List<Cart> SeedMany(int min, int max) =>
            SetCart().GenerateBetween(min, max);

        private static Faker<Cart> SetCart()
        {
            return new Faker<Cart>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.ProductId, (f) => f.Random.Number(10))
                .RuleFor(c => c.UserId, (f) => f.Random.Number(10))
                .RuleFor(c => c.Quantity, (f) => f.Random.Number(10))
                .RuleFor(c => c.Attribute, (f) => CartAttribute.Shopping)
                .RuleFor(c => c.User, (f) => UserSeeder.SeedOne())
                .RuleFor(c => c.Product, (f) => ProductSeeder.SeedOne());
        }
    }
}