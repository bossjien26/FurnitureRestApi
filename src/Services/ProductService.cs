using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.Interface;
using Repositories;
using Services.Interface;
using Services.Dto;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        private readonly IProductCategoryRepository _productCategoryRepository;

        private readonly IInventoryRepository _inventoryRepository;

        public ProductService(DbContextEntity contextEntity)
        {
            _repository = new ProductRepository(contextEntity);
            _productCategoryRepository = new ProductCategoryRepository(contextEntity);
            _inventoryRepository = new InventoryRepository(contextEntity);
        }

        public ProductService(IProductRepository genericRepository)
            => _repository = genericRepository;

        public async Task Insert(Product instance)
            => await _repository.Insert(instance);

        public async Task<Product> GetById(int id)
        {
            return await _repository.Get(x => x.Id == id);
        }

        public IQueryable<ProductInventory> GetByIdDetail(int id)
        => _repository.GetAll().Where(x => x.Id == id)
            .Join(
                _inventoryRepository.GetAll(),
                product => product.Id,
                inventory => inventory.ProductId,
                (x, y) => new { Product = x, Inventory = y }
            ).Select(
                x => new ProductInventory
                {
                    Product = x.Product,
                    Inventory = new List<Inventory>(){
                        x.Inventory
                    }
                }
            );

        public async Task<Product> GetShowProdcutById(int id)
        => await _repository.Get(x => x.Id == id && x.Inventories.Where(r => r.IsDisplay == true).Any());

        public IQueryable<Product> GetMany(int index, int size)
        {
            return _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);
        }

        public IQueryable<Product> GetShowProductMany(int index, int size)
        => _repository.GetAll().Where(x => x.Inventories.Where(r => r.IsDisplay == true).Any())
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);

        public IQueryable<Product> GetAll()
        {
            return _repository.GetAll();
        }

        public IQueryable<Product> GetProductByCategory(int index, int size, int categoryId)
        => _repository.GetAll()
                .Join(
                    _productCategoryRepository.GetAll(),
                    product => product.Id,
                    productCategory => productCategory.ProductId,
                    (x, b) => new { Product = x, ProductCategory = b }
                )
                .Join(
                    _inventoryRepository.GetAll(),
                    product => product.Product.Id,
                    inventory => inventory.ProductId,
                    (x, y) => new { Product = x, Inventory = y }
                )
                .Where(x => x.Product.ProductCategory.CategoryId == categoryId
                    && x.Inventory.IsDisplay == true && x.Inventory.IsDelete == false)
                .Select(x => x.Product.Product)
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);

        public bool CheckProductToProductCategoryIsExist(int productId, int categoryId)
        {
            return _repository.GetAll().Where(p => p.Id == productId &&
            p.ProductCategories.Where(c => c.CategoryId == categoryId && c.ProductId
            == productId && c.Category.Id == categoryId).Any()).Any();
        }
    }
}