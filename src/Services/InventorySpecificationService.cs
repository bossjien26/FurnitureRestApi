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

        public InventorySpecificationService(DbContextEntity contextEntity)
        {
            _repository = new InventorySpecificationRepository(contextEntity);
        }

        public InventorySpecificationService(IInventorySpecificationRepository genericRepository)
            => _repository = genericRepository;

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

        public bool CheckInventoryAndInventorySpecificationIsExist(int inventoryId, int specificationId)
        => _repository.GetAll().Where(x => x.Inventory.Id == inventoryId
        && x.SpecificationId == x.Specification.Id).Any();

    }
}