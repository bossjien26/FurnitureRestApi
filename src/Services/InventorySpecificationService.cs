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

        private readonly IInventoryRepository _inventoryRepository;

        public InventorySpecificationService(DbContextEntity contextEntity)
        {
            _repository = new InventorySpecificationRepository(contextEntity);
            _productSpecificationRepository = new ProductSpecificationRepository(contextEntity);
            _inventoryRepository = new InventoryRepository(contextEntity);
        }

        public InventorySpecificationService(IInventorySpecificationRepository genericRepository,
        IProductSpecificationRepository productSpecificationRepository,IInventoryRepository inventoryRepository)
        {
            _repository = genericRepository;
            _productSpecificationRepository = productSpecificationRepository;
            inventoryRepository = _inventoryRepository;
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

        public IEnumerable<Inventory> GetInventory(int productId, int[] specificationContents)
        => _repository.GetAll().Where(x => specificationContents.Contains(x.SpecificationContentId)).
            Join(
                _productSpecificationRepository.GetAll(),
                inventorySpecification => inventorySpecification.SpecificationContentId,
                productSpecification => productSpecification.Id,
                (InventorySpecification, ProductSpecification) =>
                    new { InventorySpecification, ProductSpecification }
            ).Join(
                _inventoryRepository.GetAll(),
                inventorySpecification => inventorySpecification.InventorySpecification.InventoryId,
                inventory => inventory.Id,
                (InventorySpecification,Inventory) => new {InventorySpecification,Inventory}
            )
            .Where(x => x.InventorySpecification.ProductSpecification.ProductId == productId)
            .Select(x => x.Inventory);
    }
}