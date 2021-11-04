using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using Entities;

namespace RestApi.Test.DatabaseSeeders
{
    public class SpecificationSeeder
    {
        public static Specification SeedOne() => SetSpecification().Generate();

        public static SpecificationContent SeedContentOnce(int specificationId) 
            => SetSpecificationContentById(specificationId).Generate();

        public static List<Specification> SeedMany(int min , int max) => 
                SetSpecification().GenerateBetween(min,max);

        public static List<SpecificationContent> SeedSpecificationContentMany(int min , int max) => 
                SetSpecificationContent().GenerateBetween(min,max);

        private static Faker<Specification> SetSpecification()
        {
            return new Faker<Specification>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.SpecificationContent, SetSpecificationContent().Generate(2));
        }

        private static Faker<SpecificationContent> SetSpecificationContent()
        {
            return new Faker<SpecificationContent>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.SpecificationId, (f) => f.Random.Number(10));
        }

        public static Faker<SpecificationContent> SetSpecificationContentById(int specificationId)
        {
            return new Faker<SpecificationContent>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.SpecificationId, (f) => specificationId);
        }
    }
}