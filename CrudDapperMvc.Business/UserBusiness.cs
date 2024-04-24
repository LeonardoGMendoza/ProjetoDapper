using System;
using System.Collections.Generic;
using CrudDapperMvc.Model;
using CrudDapperMvc.Model.Interfaces;

namespace CrudDapperMvc.Business
{
    public class UserBusiness
    {
        private readonly IUserRepository _userRepository;

        public UserBusiness(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Insert(User user)
        {
            // Validação do usuário, se necessário
            return _userRepository.Insert(user);
        }

        public List<User> GetAll()
        {
            return _userRepository.GetAll();
        }

        public User Get(int id)
        {
            User user = _userRepository.Get(id);

            if (user == null)
                throw new Exception($"There is no user with id: {id}");

            return user;
        }

        public bool Update(User user)
        {
            if (user == null || user.UserId <= 0 || !_userRepository.CheckIfInserted(user.UserId))
                throw new ArgumentException("Invalid user object or user does not exist");

            // Validação do usuário, se necessário
            return _userRepository.Update(user);
        }

        public bool Delete(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Please, inform a valid Id.");

            if (!_userRepository.CheckIfInserted(id))
                throw new Exception($"User with id {id} was not found.");

            return _userRepository.Delete(id);
        }

        public List<User> SearchByName(string term)
        {
            if (string.IsNullOrEmpty(term))
                throw new ArgumentException("Please, provide a search term.");

            // Chamar o método do repositório para buscar usuários pelo nome
            return _userRepository.SearchByName(term);
        }


    }
}
