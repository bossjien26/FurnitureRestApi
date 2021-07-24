using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using src.Entities;

namespace RestApi.Test.DatabaseSeeders
{
    public class UserSeeder
    {
        public static User Seedone() => SetUser().Generate();

        public static List<User> ManyUser(int min , int max) => 
                SetUser().GenerateBetween(min,max);

        private static Faker<User> SetUser()
        {
            return new Faker<User>()
                .RuleFor(c => c.Id , (f) => 0)
                .RuleFor(c => c.Mail , (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Password , (f) => f.Random.AlphaNumeric(10));
        }
   }
}