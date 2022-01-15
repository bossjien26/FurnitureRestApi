using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using Entities;

namespace RestApi.Test.DatabaseSeeders
{
    public class CategorySeeder
    {
        public static Category SeedOne() => SetParentCategory().Generate();

        public static List<Category> SeedMany(int min, int max) =>
            SetParentCategory().GenerateBetween(min, max);

        private static Faker<Category> SetParentCategory()
        {
            return new Faker<Category>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.ParentId, (f) => null)
                .RuleFor(c => c.Sequence, (f) => f.Random.Number(10))
                .RuleFor(c => c.ChildrenCategories, (f) => SetChildrenCategory().Generate(2));
        }

        private static Faker<Category> SetChildrenCategory()
        {
            return new Faker<Category>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.ParentId, (f) => f.Random.Number(10))
                .RuleFor(c => c.Sequence, (f) => f.Random.Number(10));
        }

    }
}