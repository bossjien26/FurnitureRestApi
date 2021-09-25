using System;
using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using Entities;
using Enum;

namespace RestApi.Test.DatabaseSeeders
{
    public class UserSeeder
    {
        public static User SeedOne() => SetUser().Generate();

        public static List<User> SeedMany(int min , int max) => 
                SetUser().GenerateBetween(min,max);

        private static Faker<User> SetUser()
        {
            return new Faker<User>()
                .RuleFor(c => c.Id , (f) => 0)
                .RuleFor(c => c.Mail , (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Password , (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Name , (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Role , (f) => RoleEnum.Customer)
                .RuleFor(c => c.IsVerify , (f) => false)
                .RuleFor(c => c.CreateAt , (f) => DateTime.Now)
                .RuleFor(c => c.IsDelete , (f) => false)
                .RuleFor(c => c.UserDetail , (f) => SetUserDetail());
        }
        
        private static Faker<UserDetail> SetUserDetail()
        {
            return new Faker<UserDetail>()
                .RuleFor(c => c.Id , (f) => 0)
                .RuleFor(c => c.UserId , (f) => f.Random.Number(10))
                .RuleFor(c => c.Country , (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.City , (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Street , (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.UpdateAt , (f) => DateTime.Now);
        }
   }
}