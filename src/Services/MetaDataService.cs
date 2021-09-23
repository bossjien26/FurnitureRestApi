using System.Threading.Tasks;
using DbEntity;
using Entities;
using Repositories;
using Repositories.Interface;
using Services.Interface;

namespace Services
{
    public class MetaDataService : IMetaDataService
    {
        private IMetaDataRepository _repository;

        public MetaDataService(DbContextEntity dbContextEntity)
        => _repository = new MetaDataRepository(dbContextEntity);

        public MetaDataService(IMetaDataRepository genericRepository)
        => _repository = genericRepository;

        public async Task Insert(MetaData instance)
        => await _repository.Insert(instance);

        public void Update(MetaData instance) => _repository.Update(instance);

        public async Task<MetaData> GetById(int id)
        => await _repository.Get(x => x.Id == id);
    }
}