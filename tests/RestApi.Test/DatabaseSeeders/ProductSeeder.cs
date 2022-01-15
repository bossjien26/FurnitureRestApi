using System;
using System.Collections.Generic;
using Bogus;
using Bogus.Extensions;
using Entities;

namespace RestApi.Test.DatabaseSeeders
{
    public class ProductSeeder
    {
        public static Product SeedOne() => SetProduct().Generate();

        public static Inventory SeedInventoryOne(int ProductId, int productSpecificationId, int specificationContentId)
            => SetInventory(ProductId, productSpecificationId, specificationContentId).Generate();

        public static List<Product> SeedMany(int min, int max) =>
            SetProduct().GenerateBetween(min, max);

        public static Faker<Product> SetProduct()
        {
            return new Faker<Product>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Describe, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Information, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.CreateAt, (f) => DateTime.Now)
                .RuleFor(c => c.UpdateAt, (f) => DateTime.Now)
                .RuleFor(c => c.ProductCategories, (f) => SetProductCategory().Generate(1))
                .RuleFor(c => c.ProductSpecifications, (f) => SetProductSpecification().Generate(1));
        }

        private static Faker<ProductSpecification> SetProductSpecification()
        {
            return new Faker<ProductSpecification>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Specification, (f) => SetSpecification());
        }

        private static Faker<Inventory> SetInventory(int productId, int productSpecificationId, int specificationContentId)
        {
            return new Faker<Inventory>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.ProductId, (f) => productId)
                .RuleFor(c => c.SKU, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.Price, (f) => f.Random.Number(10))
                .RuleFor(c => c.IsDisplay, (f) => true)
                .RuleFor(c => c.Quantity, (f) => f.Random.Number(10))
                .RuleFor(c => c.RelateAt, (f) => DateTime.Now)
                .RuleFor(c => c.Sequence, (f) => f.Random.Number(1))
                .RuleFor(c => c.InventorySpecifications, (f)
                => SetInventorySpecification(productSpecificationId, specificationContentId).Generate(1));
        }

        private static Faker<InventorySpecification> SetInventorySpecification(int productSpecificationId
        , int specificationContentId)
        {
            return new Faker<InventorySpecification>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.InventoryId, (f) => f.Random.Number(10))
                .RuleFor(c => c.ProductSpecificationId, (f) => productSpecificationId)
                .RuleFor(c => c.SpecificationContentId, (f) => specificationContentId);
        }

        private static Faker<Specification> SetSpecification()
        {
            return new Faker<Specification>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10));
        }

        private static Faker<SpecificationContent> SetSpecificationContent()
        {
            return new Faker<SpecificationContent>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.SpecificationId, (f) => f.Random.Number(10))
                .RuleFor(c => c.Specification, (f) => SetSpecification());
        }

        private static Faker<ProductCategory> SetProductCategory()
        {
            return new Faker<ProductCategory>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.ProductId, (f) => f.Random.Number(10))
                .RuleFor(c => c.CategoryId, (f) => f.Random.Number(10))
                .RuleFor(c => c.Category, (f) => SetCategory());
        }

        private static Faker<Category> SetCategory()
        {
            return new Faker<Category>()
                .RuleFor(c => c.Id, (f) => 0)
                .RuleFor(c => c.Name, (f) => f.Random.AlphaNumeric(10))
                .RuleFor(c => c.ParentId, (f) => 0)
                .RuleFor(c => c.Sequence, (f) => f.Random.Number(10));
        }
    }
}