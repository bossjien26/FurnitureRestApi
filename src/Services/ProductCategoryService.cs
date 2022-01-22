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
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _repository;

        public ProductCategoryService(DbContextEntity contextEntity)
        {
            _repository = new ProductCategoryRepository(contextEntity);
        }

        public ProductCategoryService(IProductCategoryRepository genericRepository)
            => _repository = genericRepository;

        public async Task Insert(ProductCategory instance)
            => await _repository.Insert(instance);

        public async Task<ProductCategory> GetById(int id)
        {
            return await _repository.Get(x => x.Id == id);
        }

        public IQueryable<ProductCategory> GetMany(int index, int size)
        {
            return _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);
        }

    }
}