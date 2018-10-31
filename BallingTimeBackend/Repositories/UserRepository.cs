using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BallingTimeBackend.Models;
using BallingTimeBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BallingTimeBackend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BallingContext _context;

        public UserRepository(BallingContext context)
        {
            _context = context;
        }

        public bool AddUser(string name, string email, string password, 
            string checkPassword, List<int> practiceDays)
        {
            if (_context.Users.Where(u => u.Email == email).Any())
                return false;
            else if (password != checkPassword)
                return false;
            else if (practiceDays.Count() < 2)
                return false;

            _context.Users.Add(new User()
            {
                Name = name,
                Email = email,
                Password = password,
                PracticeDays = JsonConvert.SerializeObject(practiceDays),
                Difficulty = _context.Difficulties.Where(x => x.DifficultyLevel == 1).First()
            });

            _context.SaveChanges();
            return true;
        }

        public bool ChangeName(string email, string newName)
        {
            if (!_context.Users.Where(u => u.Email == email).Any())
                return false;
            _context.Users.Where(u => u.Email == email).First().Name = newName;
            _context.SaveChanges();
            return true;
        }

        public bool ChangePassword(string email, string oldPassword, string newPassword, string checkPassword)
        {
            if (!_context.Users.Where(u => u.Email == email).Any())
                return false;
            else if (_context.Users.Where(u => u.Email == email).First().Password != oldPassword)
                return false;
            else if (newPassword != checkPassword)
                return false;

            _context.Users.Where(u => u.Email == email).First().Password = newPassword;
            _context.SaveChanges();
            return true;
        }

        public bool CheckUser(string email, string password)
        {
            if (!_context.Users.Where(u => u.Email == email).Any())
                return false;
            else if (_context.Users.Where(u => u.Email == email).First().Password != password)
                return false;
            return true;
        }

        public bool DeleteAccount(string email)
        {
            if (!_context.Users.Where(u => u.Email == email).Any())
                return false;
            else
            {
                _context.Users.Remove(_context.Users.Where(u => u.Email == email).First());
                _context.SaveChanges();
                return true;
            }
        }

        public User GetUserById(int userId) {
            return _context.Users.Where(user => user.Id == userId).First();
        }
    }
}
