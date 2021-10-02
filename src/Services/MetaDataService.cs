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

        public void Update(Metadata instance) => _repository.Update(instance);

        public async Task<Metadata> GetById(int id)
        => await _repository.Get(x => x.Id == id);

        public Metadata GetByCategory(MetadataCategoryEnum category, int type)
        => _repository.GetAll().Where(x => x.Type == type
           && x.Category == category).FirstOrDefault();
    }
}