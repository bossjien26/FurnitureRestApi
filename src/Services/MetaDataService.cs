using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class MetadataService : IMetadataService
    {
        private IMetadataRepository _repository;

        public MetadataService(DbContextEntity dbContextEntity)
        => _repository = new MetadataRepository(dbContextEntity);

        public MetadataService(IMetadataRepository genericRepository)
        => _repository = genericRepository;

        public async Task Insert(Metadata instance)
        => await _repository.Insert(instance);

        public async Task Update(Metadata instance) => await _repository.Update(instance);

        public async Task<Metadata> GetById(int id)
        => await _repository.Get(x => x.Id == id);

        public async Task<Metadata> GetByCategoryDetail(MetadataCategoryEnum category, int key)
        => await _repository.Get(x => x.Key == key && x.Category == category);

        public IQueryable<Metadata> GetByCategory(MetadataCategoryEnum category)
        => _repository.GetAll().Where(x => x.Category == category);
    }
}