using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories.IRepository;
using Repositories.Repository;
using Services.IService;

namespace Services.Service
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

        public async Task<Product> GetById(int Id)
        {
            return await _repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Product> GetMany(int index, int size)
        {
            return _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size);
        }

    }
}