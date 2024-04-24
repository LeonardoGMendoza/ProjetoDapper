using CrudDapperMvc.Model;
using System.Collections.Generic;

namespace CrudDapperMvc.Model.Interfaces
{
    public interface IUserRepository
    {
        public User Insert(User user);
        public List<User> GetAll();
        public User Get(int id);
        public bool Delete(int id);
        public bool Update(User user);
        public bool CheckIfInserted(int id);
        public List<User> SearchByName(string term);
    }
}
