using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IUserDetailService
    {
        Task Insert(UserDetail instance);
    }
}