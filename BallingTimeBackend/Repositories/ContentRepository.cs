using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BallingTimeBackend.Interfaces;
using BallingTimeBackend.Models;

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

        public bool AddDribblingExercise(string name, string description, string videoReference)
        {
            Regex regex = new Regex("/(youtu\\.be\\/|youtube\\.com\\/(watch\\?(.*&)?v=|(embed|v)\\/))([^\\?&\"\'>]+)/");
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

        public List<DribblingDrill> GetAllDribblingExercises()
        {
            return _context.DribblingDrills.ToList();
        }

        public Tuple<List<DribblingDrill>, Difficulty> GetFullTrainingProgramById(int userId)
        {
            //var user = _context.Users.Where(u => u.Id == userId).First();
            if (_context.Users.Where(u => u.Id == userId).Count() == 0) return null;

            var lastPractice = 
                _context
                .UserProgresses
                .Where(up => up.UserId == userId)
                .Select(userProgress => userProgress.Date)
                .Max();

            var lastPracticeDrills = 
                _context
                .UserProgresses
                .Where(up => up.UserId == userId && up.Date.Equals(lastPractice));

            var lastAccuracy = 
                lastPracticeDrills
                .Select(x => x.Accuracy);

            var lastRepsPerSec =
                lastPracticeDrills
                .Select(x => x.RepeationsPerSecond);

            var userDrillHistory =
                _context
                .UserProgresses
                .GroupBy(up => new { up.UserId, up.DribblingDrillId })
                .Select(x => x.Average(y => y.RepeationsPerSecond));

            var numberOfIncreasedReps = 

            throw new NotImplementedException();
        }
        
        public List<Tuple<DribblingDrill, double, double>> GetUserProgressById(int userId)
        {
            //TODO
            //Add dates to UserProgress(making time-based graphs); Storing Lists of progress for specific exercise;
            throw new NotImplementedException();
        }
    }
}
