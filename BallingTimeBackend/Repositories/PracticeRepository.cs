using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            if (_context
                .UserProgresses
                .Where(up => up.UserId == userId && up.Date == DateTime.Today)
                .Count() == 0)
            {
                return new List<Drill_Info>();
            }
            var user = _context.Users.Where(u => u.Id == userId).First();
            var userDifficulty = _context.Difficulties.Where(dif => dif.Id == user.DifficultyId).First();

            if (!CheckDayOfPractice(userId))
                return new List<Drill_Info>();

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
                        IsCompleted = x.isCompleted,
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
                   .Where(x => x.UserId == userId)
                   .GroupBy(up => new { up.Date })
                   .OrderBy(x => x.Key.Date)
                   .Select(x => new UserStats()
                   {
                       AverageSpeed = x.Average(y => y.Accuracy),
                       AverageRepsPerSec = x.Average(y => y.RepeationsPerSecond)
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
            drillProgress.isCompleted = true;

            _context.SaveChanges();

            return true;
        }

        private void CheckUserLevel(int userId)
        {
            List<int> allDifficultyLevels =
                _context
                .Difficulties
                .Select(x => x.DifficultyLevel)
                .ToList();

            allDifficultyLevels.Sort();

            User user =
                _context
                .Users
                .Where(u => u.Id == userId)
                .First();

            if (user.Difficulty.DifficultyLevel == allDifficultyLevels.Max()) return;

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

            List<double> lastPracticeAccuracy =
                lastPracticeDrills
                .Select(x => x.Accuracy)
                .ToList();

            List<double> lastPracticeRepsPerSec =
                lastPracticeDrills
                .Select(x => x.RepeationsPerSecond)
                .ToList();

            List<double> userOverallAverageRepPerSecAcrossAllDrills =
                _context
                .UserProgresses
                .Where(up => up.UserId == userId)
                .GroupBy(up => new { up.UserId, up.DribblingDrillId })
                .Select(x => x.Average(y => y.RepeationsPerSecond))
                .ToList();

            List<double> userOverallAverageAccuracyAcrossAllDrills =
                _context
                .UserProgresses
                .Where(up => up.UserId == userId)
                .GroupBy(up => new { up.UserId, up.DribblingDrillId })
                .Select(x => x.Average(y => y.Accuracy))
                .ToList();

            throw new NotImplementedException();
        }

        //private bool IsPracticeFinished(int userId)
        //{

        //    var user = _context
        //        .Users
        //        .Where(u => u.Id == userId)
        //        .First();

        //    DateTime lastPracticeDate =
        //        _context
        //        .UserProgresses
        //        .Where(up => up.UserId == userId)
        //        .Select(userProgress => userProgress.Date)
        //        .Max();

        //    int numberOfDrillsInlastPractice =
        //        _context
        //        .UserProgresses
        //        .Where(up => up.UserId == userId && up.Date.Equals(lastPracticeDate))
        //        .Select(up => up.DribblingDrillId)
        //        .Count();

        //    int numberOfDrillsInPractice =
        //        _context
        //        .TrainingPrograms
        //        .Where(x => x.Difficulty == user.Difficulty)
        //        .Count();

        //    throw new NotImplementedException();
        //}
    }
}
