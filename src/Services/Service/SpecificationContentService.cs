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
    public class SpecificationContentService : ISpecificationContentService
    {
        private ISpecificationContentRepository _repository;

        public SpecificationContentService(DbContextEntity contextEntity)
        {
            _repository = new SpecificationContentRepository(contextEntity);
        }

        public SpecificationContentService(ISpecificationContentRepository repository)
        {
            _repository = repository;
        }

        public async Task Insert(SpecificationContent instance)
        => await _repository.Insert(instance);

        public async Task<SpecificationContent> GetById(int id)
        {
            return await _repository.Get(x => x.Id == id);
        }

        public IEnumerable<SpecificationContent> GetMany(int index, int size)
        {
            return _repository.GetAll().Skip((index - 1) * size).Take(size).OrderByDescending(x => x.Id);
        }
    }
}