using Microsoft.AspNetCore.Mvc;
using CrudDapperMvc.Model;
using CrudDapperMvc.Model.Interfaces;
using CrudDapperMvc.Business;
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
            if (ModelState.IsValid)
            {
                _userBusiness.Insert(user);
                // Redirecionar para a action Index do UserController após a inserção bem-sucedida
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        public IActionResult Index()
        {
            var users = _userBusiness.GetAll();
            return View(users);
        }

        public IActionResult Edit(int? id)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("UserId,Name,Login,Password")] User user)
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

        public IActionResult Delete(int? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _userBusiness.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Search(string term)
        {
            var users = _userRepository.SearchByName(term);
            return PartialView("_UserListPartial", users);
        }

    }
}
