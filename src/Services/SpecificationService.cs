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
    public class SpecificationService : ISpecificationService
    {
        private ISpecificationRepository _repository;

        public SpecificationService(DbContextEntity contextEntity)
        {
            _repository = new SpecificationRepository(contextEntity);
        }

        public SpecificationService(ISpecificationRepository repository)
        {
            _repository = repository;
        }

        public async Task Insert(Specification instance)
        => await _repository.Insert(instance);

        public async Task<Specification> GetById(int id)
        {
            return await _repository.Get(x => x.Id == id);
        }

        public IEnumerable<Specification> GetMany(int index, int size)
        {
            return _repository.GetAll().Skip((index-1)*size).Take(size).OrderByDescending(x => x.Id);
        }
    }
}