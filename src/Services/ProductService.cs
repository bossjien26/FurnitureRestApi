using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.Interface;
using Repositories;
using Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(DbContextEntity contextEntity)
        {
            _repository = new ProductRepository(contextEntity);
        }

        public ProductService(IProductRepository genericRepository)
            => _repository = genericRepository;

        public async Task Insert(Product instance)
            => await _repository.Insert(instance);

        public async Task<Product> GetById(int id)
        {
            return await _repository.Get(x => x.Id == id);
        }

        public IEnumerable<Product> GetByIdDetail(int id)
        => _repository.GetAll().Where(x => x.Id == id).Include(x => x.Inventories.Where(r => r.IsDisplay == true));

        public async Task<Product> GetShowProdcutById(int id)
        => await _repository.Get(x => x.Id == id && x.Inventories.Where(r => r.IsDisplay == true).Any());

        public IEnumerable<Product> GetMany(int index, int size)
        {
            return _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);
        }

        public IEnumerable<Product> GetShowProductMany(int index, int size)
        => _repository.GetAll().Where(x => x.Inventories.Where(r => r.IsDisplay == true).Any())
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);

        public IEnumerable<Product> GetAll()
        {
            return _repository.GetAll();
        }

        public IEnumerable<Product> GetProductByCategory(int index, int size, int categoryId)
        => _repository.GetAll().Include(x => x.Inventories.Where(r => r.IsDisplay == true))
            .Include(x => x.ProductCategories.Where(r => r.CategoryId == categoryId))
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