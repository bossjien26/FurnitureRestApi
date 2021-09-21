using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.IRepository;
using Repositories.Repository;
using Services.IService;

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

        public IEnumerable<Product> GetMany(int index, int size)
        {
            return _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _repository.GetAll();
        }

        public bool CheckProductToProductCategoryIsExist(int productId, int categoryId)
        {
            return _repository.GetAll().Where(p => p.Id == productId &&
            p.ProductCategories.Where(c => c.CategoryId == categoryId && c.ProductId
            == productId && c.Category.Id == categoryId).Any()).Any();
        }

        public bool CheckProductAndProductSpecificationIsExist(int productId, int specificationId)
        {
            return _repository.GetAll().Where(x => x.Id == productId &&
            x.ProductSpecifications.Where(s => s.SpecificationId == specificationId &&
            s.ProductId == productId && s.Specification.Id == specificationId).Any()
            ).Any();
        }
    }
}