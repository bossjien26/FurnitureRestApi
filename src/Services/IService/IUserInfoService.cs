using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using src.Entities;

namespace src.Services.IService
{
    public interface IUserInfoService
    {
        void Insert(User instance);

        void Update(User instance);

        IEnumerable<User> GetAllUser();

        Task<User> FindUser(int userId);

        User GetVerifyUser(string mail,string password);
    }
}