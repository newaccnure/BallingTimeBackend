﻿using BallingTimeBackend.Data_for_frontend;
using BallingTimeBackend.Interfaces;
using BallingTimeBackend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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
            else if (String.IsNullOrWhiteSpace(name))
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

            return (_context.Users.Where(u => u.Email == email).First().Password == password);
        }

        public bool DeleteAccount(string email)
        {
            if (!_context.Users.Where(u => u.Email == email).Any())
                return false;

            _context.Users.Remove(_context.Users.Where(u => u.Email == email).First());
            _context.SaveChanges();
            return true;
        }

        public User_shortened_model GetUserById(int userId)
        {
            if (!_context.Users.Where(user => user.Id == userId).Any())
                return new User_shortened_model();

            return _context.Users.Where(user => user.Id == userId).Select(x => new User_shortened_model()
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                Password = x.Password,
                PracticeDays = JsonConvert.DeserializeObject<List<int>>(x.PracticeDays)
            }).First();
        }

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        public int GetUserIdByEmail(string email)
        {
            if (!_context.Users.Where(user => user.Email == email).Any())
                return 0;
            return _context.Users.Where(user => user.Email == email).First().Id;
        }

        public User_shortened_model GetUserByEmail(string email)
        {
            if (!_context.Users.Where(user => user.Email == email).Any())
                return new User_shortened_model();

            return _context.Users.Where(user => user.Email == email).Select(x => new User_shortened_model()
            {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                Password = x.Password,
                PracticeDays = JsonConvert.DeserializeObject<List<int>>(x.PracticeDays)
            }).First();
        }

        private void CheckUserLevel(int userId)
        {
            List<int> allDifficultyLevels =
                _context
                .Difficulties
                .Select(x => x.DifficultyLevel)
                .ToList();

            allDifficultyLevels.Sort();

            User user = GetUser(userId);

            var userDifficulty = GetUserDifficulty(user);

            if (userDifficulty.DifficultyLevel == allDifficultyLevels.Max())
                return;

            if (user.UserProgresses.Count == 0)
                return;

            DateTime lastPracticeDate =
                _context
                .UserProgresses
                .Where(up => up.UserId == userId)
                .Select(userProgress => userProgress.Date)
                .Max();

            List<UserProgress> lastPracticeDrills =
                _context
                .UserProgresses
                .Where(up => up.UserId == userId && up.Date.Equals(lastPracticeDate))
                .ToList();

            double lastPracticeAccuracy =
                lastPracticeDrills
                .Select(x => x.Accuracy)
                .ToList()
                .Average();

            double userOverallAverageAccuracyAcrossAllDrills =
                _context
                .UserProgresses
                .Where(up => up.UserId == userId)
                .GroupBy(up => new { up.UserId, up.DribblingDrillId })
                .Select(x => x.Average(y => y.Accuracy))
                .ToList()
                .Average();

            //if (lastPracticeAccuracy > userOverallAverageAccuracyAcrossAllDrills) {
            //    var user
            //    user.DifficultyId = _context.Difficulties.Where(dif=>dif.DifficultyLevel = allDifficultyLevels[allDifficultyLevels.IndexOf(user.)])
            //}
        }

        private Difficulty GetUserDifficulty(User user)
        {
            return _context.Difficulties.Where(dif => dif.Id == user.DifficultyId).First();
        }

        private User GetUser(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).First();
        }

        private bool CheckDayOfPractice(int userId)
        {
            if (_context.Users.Where(u => u.Id == userId).Count() == 0)
            {
                return false;
            }

            var user = _context.Users.Where(u => u.Id == userId).First();

            DayOfWeek today = DateTime.Now.DayOfWeek;

            List<int> practiceDays = JsonConvert.DeserializeObject<List<int>>(user.PracticeDays);

            return practiceDays.Contains((int)today);
        }
    }
}
