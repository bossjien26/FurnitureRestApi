using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities;

namespace src.Services.IService
{
    public interface IUserService
    {
        void Insert(User instance);

        void Update(User instance);

        IEnumerable<User> GetAllUser();

        Task<User> GetById(int userId);

        User GetVerifyUser(string mail,string password);
    }
}