using Microsoft.AspNetCore.Mvc;
using CrudDapperMvc.Model;
using CrudDapperMvc.Model.Interfaces;
using CrudDapperMvc.Business;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CrudDapperMvc.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserBusiness _userBusiness;

        public UserController(IUserRepository userRepository, UserBusiness userBusiness)
        {
            _userRepository = userRepository;
            _userBusiness = userBusiness;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _userBusiness.Insert(user);
                    // Redirecionar para a action Index do UserController após a inserção bem-sucedida
                    return RedirectToAction(nameof(Index));
                }
                return View(user);
            }
            catch (Exception ex)
            {
                // Lidar com a exceção de forma adequada, como registrar em log, exibir mensagem de erro personalizada, etc.
                ModelState.AddModelError("", "Ocorreu um erro ao criar o usuário.");
                return View(user);
            }
        }

        public IActionResult Index()
        {
            try
            {
                var users = _userBusiness.GetAll();
                return View(users);
            }
            catch (Exception ex)
            {
                // Lidar com a exceção de forma adequada, como registrar em log, exibir mensagem de erro personalizada, etc.
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar os usuários.";
                return View(new List<User>());
            }
        }

        public IActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = _userBusiness.Get(id.Value);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                // Lidar com a exceção de forma adequada, como registrar em log, exibir mensagem de erro personalizada, etc.
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar o usuário.";
                return View(new User());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("UserId,Name,Login,Password")] User user)
        {
            try
            {
                if (id != user.UserId)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _userBusiness.Update(user);
                    return RedirectToAction(nameof(Index));
                }
                return View(user);
            }
            catch (Exception ex)
            {
                // Lidar com a exceção de forma adequada, como registrar em log, exibir mensagem de erro personalizada, etc.
                ModelState.AddModelError("", "Ocorreu um erro ao editar o usuário.");
                return View(user);
            }
        }

        public IActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var user = _userBusiness.Get(id.Value);
                if (user == null)
                {
                    return NotFound();
                }

                return View(user);
            }
            catch (Exception ex)
            {
                // Lidar com a exceção de forma adequada, como registrar em log, exibir mensagem de erro personalizada, etc.
                ViewBag.ErrorMessage = "Ocorreu um erro ao carregar o usuário para exclusão.";
                return View(new User());
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                _userBusiness.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Lidar com a exceção de forma adequada, como registrar em log, exibir mensagem de erro personalizada, etc.
                ViewBag.ErrorMessage = "Ocorreu um erro ao excluir o usuário.";
                return View(new User());
            }
        }

        [HttpPost]
        public IActionResult Search(string term)
        {
            try
            {
                var users = _userRepository.SearchByName(term);
                return PartialView("_UserListPartial", users);
            }
            catch (Exception ex)
            {
                // Lidar com a exceção de forma adequada, como registrar em log, exibir mensagem de erro personalizada, etc.
                ViewBag.ErrorMessage = "Ocorreu um erro ao pesquisar usuários.";
                return PartialView("_UserListPartial", new List<User>());
            }
        }

        [HttpGet]
        public IActionResult GetNames()
        {
            var names = _userRepository.GetAll().Select(u => u.Name).ToList();
            return Json(names);
        }
    }
}
