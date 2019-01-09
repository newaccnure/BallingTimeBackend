using System;
using System.Collections.Generic;
using System.Linq;
using BallingTimeBackend.Interfaces;
using BallingTimeBackend.Models;
using BallingTimeBackend.Data_for_frontend;
using Newtonsoft.Json;

namespace BallingTimeBackend.Repositories
{
    public class PracticeRepository : IPracticeRepository
    {
        private readonly BallingContext _context;

        public PracticeRepository(BallingContext context)
        {
            _context = context;
        }

        public List<Drill_Info> GetFullTrainingProgramById(int userId)
        {
            if (!CheckDayOfPractice(userId))
                return new List<Drill_Info>();

            MakeTrainingProgramForToday(userId);

            var user = GetUser(userId);
            var userDifficulty = _context.Difficulties.Where(dif => dif.Id == user.DifficultyId).First(); 

            return _context
                .UserProgresses
                .Where(up => up.UserId == userId && up.Date == DateTime.Today)
                .ToList()
                .Select(x =>
                {
                    var dribblingDrill =
                        _context
                        .DribblingDrills
                        .Where(dd => dd.Id == x.DribblingDrillId)
                        .First();

                    return new Drill_Info()
                    {
                        Description = dribblingDrill.Description,
                        DifficultyId = userDifficulty.Id,
                        DrillId = dribblingDrill.Id,
                        IsCompleted = x.IsCompleted,
                        SecondsForExercise = userDifficulty.SecondsForExercise,
                        Name = dribblingDrill.Name,
                        VideoReference = dribblingDrill.VideoReference
                    };

                }).ToList();
        }

        public bool CheckDayOfPractice(int userId)
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

        public List<UserStats> GetUserStatsById(int userId)
        {
            if (_context.Users.Where(u => u.Id == userId).Count() == 0)
            {
                return new List<UserStats>();
            }
            var user = _context.Users.Where(u => u.Id == userId).First();

            List<UserStats> allPracticeDrillsStats =
                   _context
                   .UserProgresses
                   .Where(x => x.UserId == userId && x.IsCompleted)
                   .GroupBy(up => up.Date)
                   .OrderBy(x => x.Key.Date)
                   .Select(x => new UserStats()
                   {
                       AverageSpeed = x.Average(y => y.AverageSpeed),
                       AverageRepsPerSec = x.Average(y => y.RepeationsPerSecond),
                       AverageAccuracy = x.Average(y => y.Accuracy),
                       PracticeDay = x.Key.Date
                   })
                   .ToList();

            return allPracticeDrillsStats;
        }

        public bool AddDrillToCompleted(int userId, int drillId,
            double averageSpeed, double averageAccuracy, double repeatitionsPerSecond)
        {
            if (_context.Users.Where(user => user.Id == userId).Count() == 0 ||
                _context.DribblingDrills.Where(drill => drill.Id == drillId).Count() == 0) {
                return false;
            }

            var drillProgress = 
                _context
                .UserProgresses
                .Where(up => up.UserId == userId 
                            && up.DribblingDrillId == drillId 
                            && up.Date == DateTime.Today)
                .First();

            drillProgress.Accuracy = averageAccuracy;
            drillProgress.AverageSpeed = averageSpeed;
            drillProgress.RepeationsPerSecond = repeatitionsPerSecond;
            drillProgress.IsCompleted = true;

            _context.SaveChanges();

            return true;
        }

        public DrillStats GetDrillStatsById(int userId, int drillId)
        {
            if (_context.Users.Where(user => user.Id == userId).Count() == 0 ||
                _context.DribblingDrills.Where(drill => drill.Id == drillId).Count() == 0)
            {
                return new DrillStats();
            }

            var drillProgress =
                _context
                .UserProgresses
                .Where(up => up.UserId == userId
                            && up.DribblingDrillId == drillId
                            && up.Date == DateTime.Today)
                .First();

            return new DrillStats() {
                Accuracy = drillProgress.Accuracy,
                AverageSpeed = drillProgress.AverageSpeed,
                RepsPerSec = drillProgress.RepeationsPerSecond
            };
        }

        private User GetUser(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).First();
        }

        private void CheckUserLevel(int userId)
        {
            var difficulties = _context.Difficulties;
            var userProgresses = _context.UserProgresses;

            if (!userProgresses.Where(up => up.UserId == userId).Any())
                return;

            List<int> allDifficultyLevels =
                difficulties
                .Select(x => x.DifficultyLevel)
                .ToList();


            User user =
                _context
                .Users
                .Where(u => u.Id == userId)
                .First();

            Difficulty userDifficulty =
                difficulties
                .Where(x => x.Id == user.DifficultyId)
                .First();

            if (userDifficulty.DifficultyLevel == allDifficultyLevels.Max()) return;

            DateTime lastPracticeDate =
                userProgresses
                .Where(up => up.UserId == userId)
                .Select(userProgress => userProgress.Date)
                .Max();

            List<UserProgress> lastPracticeDrills =
                userProgresses
                .Where(up => up.UserId == userId && up.Date.Equals(lastPracticeDate))
                .ToList();

            double lastPracticeAccuracy =
                lastPracticeDrills
                .Select(x => x.Accuracy)
                .ToList()
                .Average();

            double lastPracticeRepsPerSec =
                lastPracticeDrills
                .Select(x => x.RepeationsPerSecond)
                .ToList()
                .Average();

            double userOverallAverageRepPerSecAcrossAllDrills =
                userProgresses
                .Where(up => up.UserId == userId)
                .GroupBy(up => new { up.UserId, up.DribblingDrillId })
                .Select(x => x.Average(y => y.RepeationsPerSecond))
                .ToList()
                .Average();

            double userOverallAverageAccuracyAcrossAllDrills =
                userProgresses
                .Where(up => up.UserId == userId)
                .GroupBy(up => new { up.UserId, up.DribblingDrillId })
                .Select(x => x.Average(y => y.Accuracy))
                .ToList()
                .Average();

            if (lastPracticeAccuracy > userOverallAverageAccuracyAcrossAllDrills
                && lastPracticeRepsPerSec > userOverallAverageRepPerSecAcrossAllDrills)
            {
                allDifficultyLevels.Sort();
                var index = allDifficultyLevels
                    .FindIndex(0, allDifficultyLevels.Count, x => x == userDifficulty.DifficultyLevel);
                Difficulty newDifficulty = difficulties
                    .Where(x => x.DifficultyLevel == allDifficultyLevels[index + 1]).First();
                user.Difficulty = newDifficulty;
                _context.SaveChanges();
            }

        }

        private void MakeTrainingProgramForToday(int userId)
        {
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

            foreach (var dribblingDrill in dribblingDrills)
            {
                _context.UserProgresses.Add(new UserProgress()
                {
                    IsCompleted = false,
                    Date = DateTime.Today,
                    DribblingDrillId = dribblingDrill.DribblingDrillId,
                    UserId = userId
                });
            }
            _context.SaveChanges();
        }

        public bool PracticeWasStarted(int userId)
        {
            return _context.UserProgresses.Where(up => up.UserId == userId && up.Date == DateTime.Today).Any();
        }
    }
}
