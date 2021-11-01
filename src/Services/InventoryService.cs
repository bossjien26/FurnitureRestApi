using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;

        public InventoryService(DbContextEntity contextEntity)
        {
            _repository = new InventoryRepository(contextEntity);
        }

        public InventoryService(IInventoryRepository genericRepository)
            => _repository = genericRepository;

        public async Task Insert(Inventory instance) => await _repository.Insert(instance);

        public async Task<Inventory> GetById(int id) => await _repository.Get(x => x.Id == id);

        public async Task<Inventory> GetShowById(int id) => await _repository.Get(x => x.Id == id
            && x.IsDisplay == true && x.IsDelete == false);

        public IEnumerable<Inventory> GetJoinProductAndSpecification(int id)
        => _repository.GetAll()
            .Include(x => x.Product)
            .Include(x => x.InventorySpecifications)
            .ThenInclude(x => x.SpecificationContent)
            .ThenInclude(x => x.Specification)
            .Where(x => x.Id == id);

        public IEnumerable<Inventory> GetMany(int index, int size)
            => _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);

        public IEnumerable<Inventory> GetShowMany(int index, int size)
            => _repository.GetAll()
                .Include(x => x.Product)
                .Include(x => x.InventorySpecifications)
                .ThenInclude(x => x.SpecificationContent)
                .ThenInclude(x => x.Specification)
                .Where(x => x.IsDisplay == true && x.IsDelete == false)
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);

        public void Update(Inventory instance) => _repository.Update(instance);

        public void Delete(Inventory instance) => _repository.Delete(instance);
    }
}