using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.Interface;
using Repositories;
using Services.Interface;

namespace Services
{
    public class InventorySpecificationService : IInventorySpecificationService
    {
        private readonly IInventorySpecificationRepository _repository;

        private readonly IProductSpecificationRepository _productSpecificationRepository;

        public InventorySpecificationService(DbContextEntity contextEntity)
        {
            _repository = new InventorySpecificationRepository(contextEntity);
            _productSpecificationRepository = new ProductSpecificationRepository(contextEntity);
        }

        public InventorySpecificationService(IInventorySpecificationRepository genericRepository,
        IProductSpecificationRepository productSpecificationRepository)
        {
            _repository = genericRepository;
            _productSpecificationRepository = productSpecificationRepository;
        }

        public async Task Insert(InventorySpecification instance)
            => await _repository.Insert(instance);

        public async Task<InventorySpecification> GetById(int id)
        {
            return await _repository.Get(x => x.Id == id);
        }

        public IEnumerable<InventorySpecification> GetMany(int index, int size)
        {
            return _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);
        }

        public async Task<bool> CheckInventoryAndInventorySpecificationIsExist(int inventoryId, int specificationContentId)
        => await _repository.Get(x => x.InventoryId == inventoryId && x.SpecificationContentId == specificationContentId) != null;

        //TODO:show inventory
        public IEnumerable<int> GetInventory(int productId, int[] specificationContents)
        => _repository.GetAll().Where(x => specificationContents.Contains(x.SpecificationContentId)).
            Join(
                _productSpecificationRepository.GetAll(),
                inventorySpecification => inventorySpecification.SpecificationContentId,
                productSpecification => productSpecification.Id,
                (InventorySpecification, ProductSpecification) =>
                    new { InventorySpecification, ProductSpecification }
            )
            .Where(x => x.ProductSpecification.ProductId == productId)
            .Select(x => x.InventorySpecification.InventoryId);
    }
}