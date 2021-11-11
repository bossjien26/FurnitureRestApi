using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class ProductSpecificationService : IProductSpecificationService
    {

        private readonly IProductSpecificationRepository _repository;

        public ProductSpecificationService(DbContextEntity contextEntity)
        => _repository = new ProductSpecificationRepository(contextEntity);

        public ProductSpecificationService(IProductSpecificationRepository genericRepository)
            => _repository = genericRepository;

        public async Task Insert(ProductSpecification instance)
        => await _repository.Insert(instance);

        public IEnumerable<ProductSpecification> GetMany(int productId)
        => _repository.GetAll().Where(x => x.ProductId == productId);


        private IIncludableQueryable<ProductSpecification, ICollection<SpecificationContent>> GetOneJoinSpecification(int ProductId)
        => _repository.GetAll().Where(x => x.ProductId == ProductId)
            .Include(x => x.Specification)
            .ThenInclude(x => x.SpecificationContent);

        public IEnumerable<ProductSpecification> GetOneJoinSpecificationByProductId(int productId, int offset, int[] inventoryId)
        {
            var iEnumerable = GetOneJoinSpecification(productId);
            if (inventoryId.Count() > 0)
            {
                return iEnumerable.Include(x => x.InventorySpecifications.Where(y =>
                    inventoryId.All(z => z == y.ProductSpecificationId)
                )).Skip(offset)
                .Take(1);
            }
            return iEnumerable.Include(x => x.InventorySpecifications)
            .Skip(offset)
            .Take(1);
        }
    }
}