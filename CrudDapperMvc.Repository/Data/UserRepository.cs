using CrudDapperMvc.Model;
using CrudDapperMvc.Model.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace CrudDapperMvc.Repository.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public User Insert(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                // Verificar se o login já existe
                var existingUser = GetByLogin(user.Login);
                if (existingUser != null)
                {
                    throw new Exception("O login já está em uso por outro usuário.");
                }

                // Se o login for único, proceder com a inserção
                var parameters = new DynamicParameters();
                parameters.Add("@Name", user.Name);
                parameters.Add("@Login", user.Login);
                parameters.Add("@Password", user.Password);

                var sql = "INSERT INTO [dbo].[User] (Name, Login, Password) VALUES (@Name, @Login, @Password); SELECT CAST(SCOPE_IDENTITY() as int)";
                int userId = connection.Query<int>(sql, parameters).Single();
                user.UserId = userId;
                return user;
            }
        }

        public User GetByLogin(string login)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM [dbo].[User] WHERE Login = @Login";
                return connection.Query<User>(sql, new { Login = login }).FirstOrDefault();
            }
        }

        public List<User> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM [dbo].[User]";
                return connection.Query<User>(sql).ToList();
            }
        }

        public User Get(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM [dbo].[User] WHERE UserId = @UserId";
                return connection.Query<User>(sql, new { UserId = id }).FirstOrDefault();
            }
        }

        public bool Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "DELETE FROM [dbo].[User] WHERE UserId = @UserId";
                int rowsAffected = connection.Execute(sql, new { UserId = id });
                return rowsAffected > 0;
            }
        }

        public bool Update(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", user.UserId);
                parameters.Add("@Name", user.Name);
                parameters.Add("@Login", user.Login);
                parameters.Add("@Password", user.Password);

                var sql = "UPDATE [dbo].[User] SET Name = @Name, Login = @Login, Password = @Password WHERE UserId = @UserId";
                int rowsAffected = connection.Execute(sql, parameters);
                return rowsAffected > 0;
            }
        }

        public List<User> SearchByName(string term)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM [dbo].[User] WHERE Name LIKE @Term";
                return connection.Query<User>(sql, new { Term = $"%{term}%" }).ToList();
            }
        }

        public bool CheckIfInserted(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = "SELECT COUNT(1) FROM [dbo].[User] WHERE UserId = @UserId";
                int count = connection.Query<int>(sql, new { UserId = id }).FirstOrDefault();
                return count > 0;
            }
        }
    }
}
