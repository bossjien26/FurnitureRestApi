using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using Entities;

namespace RestApi.Test.DatabaseSeeders
{
    public class CategorySeeder
    {
        public static Category SeedOne() => SetCategory().Generate();

        public static List<Category> SeedMany(int min, int max) =>
            SetCategory().GenerateBetween(min, max);

        private static Faker<Category> SetCategory()
        {
            return new Faker<Category>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.ChildrenId, (f) => 0)
                .RuleFor(c => c.Sequence, (f) => f.Random.Number(10));
        }
    }
}