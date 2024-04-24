using CrudDapperMvc.Model;
using System.Collections.Generic;

namespace CrudDapperMvc.Model.Interfaces
{
    public interface IUserRepository
    {
        User Insert(User user);
        List<User> GetAll();
        User Get(int id);
        bool Delete(int id);
        bool Update(User user);
        bool CheckIfInserted(int id);

        List<User> SearchByName(string term);

    }
}
