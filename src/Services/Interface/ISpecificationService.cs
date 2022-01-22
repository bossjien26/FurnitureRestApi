using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface ISpecificationService
    {
        Task Insert(Specification specification);

        Task<Specification> GetById(int id);

        IQueryable<Specification> GetMany(int index, int size);
    }
}