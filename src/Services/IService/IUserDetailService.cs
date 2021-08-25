using System.Threading.Tasks;
using Entities;

namespace Services.IService
{
    public interface IUserDetailService
    {
        Task Insert(UserDetail instance);
    }
}