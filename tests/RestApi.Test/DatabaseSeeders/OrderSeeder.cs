using System;
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
                .RuleFor(c => c.Receiver, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.ReceiverMail, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Payer, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.User, (f) => SetUser())
                .RuleFor(c => c.OrderProducts, (f) => SetOrderProduct().Generate(5))
                .RuleFor(c => c.OrderPay, (f) => SetOrderPay());
        }

        private static Faker<OrderPay> SetOrderPay()
        {
            return new Faker<OrderPay>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Terms, (f) => (byte)PaymentTerms.Bank);
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

        private static Faker<User> SetUser()
        {
            return new Faker<User>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Mail, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Password, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Role, (f) => (byte)UserRole.Customer)
                .RuleFor(c => c.IsVerify, (f) => false)
                .RuleFor(c => c.Create, (f) => DateTime.Now)
                .RuleFor(c => c.IsDelete, (f) => false)
                .RuleFor(c => c.UserDetail, (f) => SetUserDetail());
        }

        private static Faker<UserDetail> SetUserDetail()
        {
            return new Faker<UserDetail>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.UserId, (f) => f.Random.Number(10))
                .RuleFor(c => c.Country, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.City, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Street, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.UpdateAt, (f) => DateTime.Now);
        }
    }
}