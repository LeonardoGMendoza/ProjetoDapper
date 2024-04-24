using CrudDapperMvc.Business;
using CrudDapperMvc.Model;
using CrudDapperMvc.Model.Interfaces;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CrudDapperMvc.Teste
{
    [TestClass]
    public class UserBusinessTest
    {
        private readonly Mock<IUserRepository> _userRepository = new();

        [TestMethod]
        public void UserBusiness_Insert_OK()
        {
            User user = GenerateDefaultUser();

            _userRepository.Setup(s => s.Insert(It.IsAny<User>())).Returns(user);

            UserBusiness business = new(_userRepository.Object);

            var insertedUser = business.Insert(user);

            Assert.IsNotNull(insertedUser);
            Assert.AreEqual(user.UserId, insertedUser.UserId);
        }

        [TestMethod]
        public void UserBusiness_GetAll_OK()
        {
            User user = GenerateDefaultUser();

            _userRepository.Setup(s => s.GetAll()).Returns(new List<User>() { user });

            UserBusiness business = new(_userRepository.Object);

            var users = business.GetAll();

            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count > 0);
        }

        [TestMethod]
        public void UserBusiness_Delete_OK()
        {
            _userRepository.Setup(s => s.Delete(It.IsAny<int>())).Returns(true);
            _userRepository.Setup(s => s.CheckIfInserted(It.IsAny<int>())).Returns(true);

            UserBusiness business = new(_userRepository.Object);

            var isDeleted = business.Delete(1); // Substitua 1 pelo ID do usuário que você deseja deletar

            Assert.IsTrue(isDeleted);
        }

        [TestMethod]
        public void UserBusiness_Update_OK()
        {
            User user = GenerateDefaultUser();

            _userRepository.Setup(s => s.Update(It.IsAny<User>())).Returns(true);
            _userRepository.Setup(s => s.CheckIfInserted(It.IsAny<int>())).Returns(true);

            UserBusiness business = new(_userRepository.Object);

            var isUpdated = business.Update(user);

            Assert.IsTrue(isUpdated);
        }

        [TestMethod]
        public void UserBusiness_Get_OK()
        {
            User user = GenerateDefaultUser();

            _userRepository.Setup(s => s.Get(It.IsAny<int>())).Returns(user);

            UserBusiness business = new(_userRepository.Object);

            var userObtained = business.Get(1); // Substitua 1 pelo ID do usuário que você deseja obter

            Assert.IsNotNull(userObtained);
            Assert.AreNotEqual(0, userObtained.UserId); // Verifica se o ID obtido é diferente de 0
        }

        [TestMethod]
        public void UserBusiness_Insert_InvalidUser_NOK()
        {
            User user = GenerateDefaultUser();
            user.Age = 0;

            UserBusiness business = new(_userRepository.Object);

            Assert.ThrowsException<ValidationException>(() => business.Insert(user));
        }

        [TestMethod]
        public void UserBusiness_Get_InvalidUser_NOK()
        {
            _userRepository.Setup(s => s.Get(It.IsAny<int>())).Returns((User)null);

            UserBusiness business = new(_userRepository.Object);

            Assert.ThrowsException<Exception>(() => business.Get(1)); // Substitua 1 pelo ID do usuário que você deseja obter
        }

        [TestMethod]
        public void UserBusiness_Delete_InvalidId_NOK()
        {
            UserBusiness business = new(_userRepository.Object);
            Assert.ThrowsException<Exception>(() => business.Delete(0)); // Substitua 0 pelo ID inválido do usuário que você deseja deletar
        }

        [TestMethod]
        public void UserBusiness_Delete_InvalidUser_NOK()
        {
            _userRepository.Setup(s => s.CheckIfInserted(It.IsAny<int>())).Returns(false);

            UserBusiness business = new(_userRepository.Object);

            Assert.ThrowsException<Exception>(() => business.Delete(0)); // Substitua 0 pelo ID inválido do usuário que você deseja deletar
        }

        [TestMethod]
        public void UserBusiness_Update_InvalidId_NOK()
        {
            User user = GenerateDefaultUser();
            user.UserId = 0; // Define um ID inválido

            UserBusiness business = new(_userRepository.Object);

            Assert.ThrowsException<Exception>(() => business.Update(user));
        }

        [TestMethod]
        public void UserBusiness_Update_UserNotExists_NOK()
        {
            User user = GenerateDefaultUser();

            _userRepository.Setup(s => s.CheckIfInserted(It.IsAny<int>())).Returns(false);
            UserBusiness business = new(_userRepository.Object);

            Assert.ThrowsException<Exception>(() => business.Update(user));
        }

        [TestMethod]
        public void UserBusiness_Update_InvalidUser_NOK()
        {
            User user = GenerateDefaultUser();
            user.Name = "";

            _userRepository.Setup(s => s.CheckIfInserted(It.IsAny<int>())).Returns(true);
            UserBusiness business = new(_userRepository.Object);

            Assert.ThrowsException<ValidationException>(() => business.Update(user));
        }

        #region Default Values
        private static User GenerateDefaultUser()
        {
            return new()
            {
                Name = "Leonardo",
                UserId = 1, // Defina o ID padrão do usuário
                Login = "leonardo@example.com", // Defina o login padrão do usuário
                Password = "senha123" // Defina a senha padrão do usuário
            };
        }
        #endregion
    }
}
