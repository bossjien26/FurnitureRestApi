using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using Entities;
using Enum;

namespace RestApi.Test.DatabaseSeeders
{
    public class OrderSeeder
    {
        public static Order SeedOne() => SetOrder().Generate();

        public static List<Order> SeedMany(int min, int max) =>
            SetOrder().GenerateBetween(min, max);
        private static Faker<Order> SetOrder()
        {
            return new Faker<Order>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.UserId, (f) => f.Random.Number(10))
                .RuleFor(c => c.Country, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.City, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Street, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Recipient, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.RecipientMail, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Sender, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.User, (f) => UserSeeder.SeedOne())
                .RuleFor(c => c.OrderProducts, (f) => SetOrderProduct().Generate(5))
                .RuleFor(c => c.OrderPay, (f) => SetOrderPay());
        }

        private static Faker<OrderPay> SetOrderPay()
        {
            return new Faker<OrderPay>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Terms, (f) => PaymentTypeEnum.Bank);
        }

        private static Faker<OrderProduct> SetOrderProduct()
        {
            return new Faker<OrderProduct>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Price, (f) => f.Random.Number(10))
                .RuleFor(c => c.ProductId, (f) => f.Random.Number(10))
                .RuleFor(c => c.ProductName, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Quality, (f) => f.Random.Number(10))
                .RuleFor(c => c.Specification, (f) => f.Random.AlphaNumeric(10));
        }
    }
}