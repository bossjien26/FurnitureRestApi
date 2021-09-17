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
    public class ProductSpecificationService : IProductSpecificationService
    {
        private readonly IProductSpecificationRepository _repository;

        public ProductSpecificationService(DbContextEntity contextEntity)
        {
            _repository = new ProductSpecificationRepository(contextEntity);
        }

        public ProductSpecificationService(IProductSpecificationRepository genericRepository)
            => _repository = genericRepository;

        public async Task Insert(ProductSpecification instance)
            => await _repository.Insert(instance);

        public async Task<ProductSpecification> GetById(int id)
        {
            return await _repository.Get(x => x.Id == id);
        }

        public IEnumerable<ProductSpecification> GetMany(int index, int size)
        {
            return _repository.GetAll()
                .Skip((index - 1) * size)
                .Take(size)
                .OrderByDescending(x => x.Id);
        }
    }
}