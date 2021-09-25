using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using Entities;

namespace RestApi.Test.DatabaseSeeders
{
    public class MetaDataSeeder
    {
        public static MetaData SeedOne() => SetMetaData().Generate();

        public static List<MetaData> SeedMany(int min, int max) =>
            SetMetaData().GenerateBetween(min, max);

        private static Faker<MetaData> SetMetaData()
        {
            return new Faker<MetaData>()
            .RuleFor(c => c.Category, (f) => Enum.MetaDataCategoryEnum.Pay)
            .RuleFor(c => c.Key, (f) => f.Random.AlphaNumeric(100))
            .RuleFor(c => c.Type, (f) => f.Random.Number(100, 1000))
            .RuleFor(c => c.Value, (f) => f.Random.AlphaNumeric(100));
        }
    }
}