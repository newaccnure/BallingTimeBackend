using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BallingTimeBackend.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace BallingTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class UserController : Controller
    {
        public IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        [Route("addUser")]
        public JsonResult AddUser(string name, string email, string password,
            string checkPassword, List<int> practiceDays)
        {
            return Json(_userRepository.AddUser(name, email, password, checkPassword, practiceDays));
        }

        [HttpPost]
        [Route("changePassword")]
        public JsonResult ChangePassword(string email, string oldPassword, string newPassword, string checkPassword)
        {
            return Json(_userRepository.ChangePassword(email, oldPassword, newPassword, checkPassword));
        }

        [HttpPost]
        [Route("checkUser")]
        public JsonResult CheckUser(string email, string password)
        {
            return Json(_userRepository.CheckUser(email, password));
        }

        [HttpPost]
        [Route("changeName")]
        public JsonResult ChangeName(string email, string name)
        {
            return Json(_userRepository.ChangeName(email, name));
        }

        [HttpPost]
        [Route("deleteAccount")]
        public JsonResult DeleteAccount(string email)
        {
            return Json(_userRepository.DeleteAccount(email));
        }

        [HttpPost]
        [Route("getUserById")]
        public JsonResult GetUserById(int userId)
        {
            return Json(_userRepository.GetUserById(userId));
        }
        [HttpGet]
        [Route("getAllUsers")]
        public JsonResult GetAllUsers() {
            return Json(_userRepository.GetAllUsers());
        }
    }
}