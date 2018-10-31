using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BallingTimeBackend.Interfaces;
using BallingTimeBackend.Models;
using BallingTimeBackend.Data_for_frontend;
using Newtonsoft.Json;

namespace BallingTimeBackend.Repositories
{
    public class ContentRepository : IContentRepository
    {

        private readonly BallingContext _context;

        public ContentRepository(BallingContext context)
        {
            _context = context;
        }

        public bool AddDifficulty(int secondsForExercise, int difficultyLevel)
        {
            if (_context.Difficulties.Where(dif => dif.DifficultyLevel.Equals(difficultyLevel)).Any()) {
                return false;
            }
            else if (secondsForExercise <= 0 || difficultyLevel <= 0) {
                return false;
            }
            _context.Difficulties.Add(new Difficulty()
            {
                SecondsForExercise = secondsForExercise,
                DifficultyLevel = difficultyLevel
            });
            return true;
        }

        public List<Difficulty> GetAllDifficulties()
        {
            return _context.Difficulties.ToList();
        }

        public bool AddDribblingDrill(string name, string description, string videoReference)
        {
            Regex regex = new Regex("(youtu\\.be\\/|youtube\\.com\\/(watch\\?(.*&)?v=|(embed|v)\\/))([^\\?&\"\'>]+)");
            if (_context.DribblingDrills.Where(de => de.Name.Equals(name)).Any())
            {
                return false;
            }
            else if (description.Equals(String.Empty) 
                    || !regex.IsMatch(videoReference))
            {
                return false;
            }

            _context.DribblingDrills.Add(new DribblingDrill()
            {
                Name = name,
                Description = description,
                VideoReference = videoReference
            });
            return true;
        }

        public List<DribblingDrill> GetAllDribblingDrills()
        {
            return _context.DribblingDrills.ToList();
        }

        public List<Drill_Info> GetFullTrainingProgramById(int userId)
        {
            if (_context.Users.Where(u => u.Id == userId).Count() == 0) return new List<Drill_Info>();

            var user = _context.Users.Where(u => u.Id == userId).First();

            List<DribblingDrill> allPracticeDrills =
                   _context
                   .TrainingPrograms
                   .Where(x => x.Difficulty == user.Difficulty)
                   .Select(x => x.DribblingDrill)
                   .ToList();

            if (!IsPracticeFinished(userId)) {
                DateTime lastPracticeDate =
                    _context
                    .UserProgresses
                    .Where(up => up.UserId == userId)
                    .Select(userProgress => userProgress.Date)
                    .Max();

                List<DribblingDrill> drillsInlastPractice =
                    _context
                    .UserProgresses
                    .Where(up => up.UserId == userId && up.Date.Equals(lastPracticeDate))
                    .Select(up => up.DribblingDrill)
                    .ToList();

                return new List<Drill_Info>(
                        allPracticeDrills
                        .Select(x => new Drill_Info()
                            {
                                IsCompleted = drillsInlastPractice.Contains(x),
                                SecondsForExercise = user.Difficulty.SecondsForExercise,
                                DifficultyId = user.Difficulty.Id,
                                DrillId = x.Id,
                                Name = x.Name,
                                Description = x.Description,
                                VideoReference = x.VideoReference
                            }));
            }

            CheckUserLevel(userId);

            return new List<Drill_Info>(
                        allPracticeDrills
                        .Select(x => new Drill_Info()
                        {
                            IsCompleted = false,
                            SecondsForExercise = user.Difficulty.SecondsForExercise,
                            DifficultyId = user.Difficulty.Id,
                            DrillId = x.Id,
                            Name = x.Name,
                            Description = x.Description,
                            VideoReference = x.VideoReference
                        })); 
        }

        public bool CheckDayOfPractice(int userId) {

            if (_context.Users.Where(u => u.Id == userId).Count() == 0) {
                return false;
            }

            var user = _context.Users.Where(u => u.Id == userId).First();

            DayOfWeek today = DateTime.Now.DayOfWeek;

            List<int> practiceDays = JsonConvert.DeserializeObject<List<int>>(user.PracticeDays);

            return practiceDays.Contains((int)today);
        }

        public List<UserStats> GetUserStatsById(int userId)
        {
            if (_context.Users.Where(u => u.Id == userId).Count() == 0) {
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

        private void CheckUserLevel(int userId) {
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

        private bool IsPracticeFinished(int userId) {
            var user = _context
                .Users
                .Where(u => u.Id == userId)
                .First();

            DateTime lastPracticeDate =
                _context
                .UserProgresses
                .Where(up => up.UserId == userId)
                .Select(userProgress => userProgress.Date)
                .Max();

            int numberOfDrillsInlastPractice =
                _context
                .UserProgresses
                .Where(up => up.UserId == userId && up.Date.Equals(lastPracticeDate))
                .Select(up => up.DribblingDrillId)
                .Count();

            int numberOfDrillsInPractice =
                _context
                .TrainingPrograms
                .Where(x => x.Difficulty == user.Difficulty)
                .Count();

            return numberOfDrillsInPractice == numberOfDrillsInlastPractice;
        }

    }
}
