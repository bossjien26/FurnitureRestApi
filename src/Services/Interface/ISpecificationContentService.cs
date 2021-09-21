using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface ISpecificationContentService
    {
        Task Insert(SpecificationContent specification);

        Task<SpecificationContent> GetById(int id);

        IEnumerable<SpecificationContent> GetMany(int index, int size);
    }
}