using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.IService
{
    public interface ISpecificationService
    {
        Task Insert(Specification specification);

        Task<Specification> GetById(int id);

        IEnumerable<Specification> GetMany(int index, int size);
    }
}