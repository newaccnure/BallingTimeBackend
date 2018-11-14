using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BallingTimeBackend.Models;
using BallingTimeBackend.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BallingTimeBackend.Data_for_frontend;

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

            int userId = GetUserIdByEmail(email);

            if (CheckDayOfPractice(userId)) {
                MakeTrainingProgramForToday(userId);
            }

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

        public User_shortened_model GetUserById(int userId) {
            return _context.Users.Where(user => user.Id == userId).Select(x => new User_shortened_model() {
                Id = x.Id,
                Email = x.Email,
                Name = x.Name,
                Password = x.Password,
                PracticeDays = JsonConvert.DeserializeObject<List<int>>(x.PracticeDays)
            }).First();
        }
        
        public List<User> GetAllUsers() {
            return _context.Users.ToList();
        }

        public int GetUserIdByEmail(string email)
        {
            return _context.Users.Where(user => user.Email == email).First().Id;
        }

        private void MakeTrainingProgramForToday(int userId) {
            CheckUserLevel(userId);
            User user = GetUser(userId);

            if (_context
                .UserProgresses
                .Where(up => up.UserId == userId && up.Date == DateTime.Today)
                .Count() > 0)
                return;

            var dribblingDrills = 
                _context
                .TrainingPrograms
                .Where(tp => tp.DifficultyId == user.DifficultyId);

            foreach (var dribblingDrill in dribblingDrills) {
                _context.UserProgresses.Add(new UserProgress() {
                    IsCompleted = false,
                    Date = DateTime.Today,
                    DribblingDrillId = dribblingDrill.DribblingDrillId,
                    UserId = userId
                });
            }
            _context.SaveChanges();
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
        private Difficulty GetUserDifficulty(User user) {
            return _context.Difficulties.Where(dif => dif.Id == user.DifficultyId).First();
        }
        private User GetUser(int userId) {
            return _context.Users.Where(u => u.Id == userId).First();
        }
        private bool CheckDayOfPractice(int userId) {
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
