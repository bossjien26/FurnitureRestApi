using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using Entities;

namespace RestApi.Test.DatabaseSeeders
{
    public class MetadataSeeder
    {
        public static Metadata SeedOne() => SetMetadata().Generate();

        public static List<Metadata> SeedMany(int min, int max) =>
            SetMetadata().GenerateBetween(min, max);

        private static Faker<Metadata> SetMetadata()
        {
            return new Faker<Metadata>()
            .RuleFor(c => c.Category, (f) => Enum.MetadataCategoryEnum.Pay)
            .RuleFor(c => c.Key, (f) => f.Random.Number(100))
            .RuleFor(c => c.Value, (f) => f.Random.AlphaNumeric(100));
        }
    }
}