using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Interface;
using Services.Dto;
using Services.Interface;

namespace Services
{
    public class InventoryService : IInventoryService
    {
        private readonly IInventoryRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IInventorySpecificationRepository _inventorySpecificationRepository;
        
        public InventoryService(DbContextEntity contextEntity)
        {
            _repository = new InventoryRepository(contextEntity);
            _productRepository = new ProductRepository(contextEntity);
            _inventorySpecificationRepository = new InventorySpecificationRepository(contextEntity);
        }

        public InventoryService(IInventoryRepository genericRepository)
            => _repository = genericRepository;

        public async Task Insert(Inventory instance) => await _repository.Insert(instance);

        public async Task<Inventory> GetById(int id) => await _repository.Get(x => x.Id == id);

        public async Task<Inventory> GetShowById(int id) => await _repository.Get(x => x.Id == id
            && x.IsDisplay == true && x.IsDelete == false);

        public IQueryable<ProductToInventory> GetJoinProduct(int id)
        => _repository.GetAll().Where(x => x.Id == id)
            .Join(
                _inventorySpecificationRepository.GetAll(),
                inventory => inventory.Id,
                inventorySpecification => inventorySpecification.InventoryId,
                (x, y) => new { Inventory = x, InventorySpecification = y }
            ).Join(
                _productRepository.GetAll(),
                inventory => inventory.Inventory.ProductId,
                product => product.Id,
                (x, y) => new { Inventory = x, Product = y }
            )
            .Select(
                x => new ProductToInventory
                {
                    InventoryId = x.Inventory.Inventory.Id,
                    ProductName = x.Product.Name,
                    Price = x.Inventory.Inventory.Price
                }
            );

        public IEnumerable<Inventory> GetMany(int index, int size)
            => _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);

        public IEnumerable<Inventory> GetShowMany(int index, int size)
            => _repository.GetAll()
                .Where(x => x.IsDisplay == true && x.IsDelete == false)
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);

        public void Update(Inventory instance) => _repository.Update(instance);

        public void Delete(Inventory instance) => _repository.Delete(instance);
    }
}