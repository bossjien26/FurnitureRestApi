using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface ISpecificationContentService
    {
        Task Insert(SpecificationContent specification);

        Task<SpecificationContent> GetById(int id);

        IQueryable<SpecificationContent> GetMany(int index, int size);
    }
}