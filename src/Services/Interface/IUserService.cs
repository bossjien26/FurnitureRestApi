using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Interface
{
    public interface IUserService
    {
        Task Insert(User instance);

        void Update(User instance);

        IEnumerable<User> GetAll();

        IEnumerable<User> GetMany(int index, int size);

        Task<User> GetById(int userId);

        User GetVerifyUser(string mail,string password);

        User SearchUserMail(string mail);
    }
}