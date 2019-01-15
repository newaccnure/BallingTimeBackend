using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
            _context.SaveChanges();
            return true;
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
            _context.SaveChanges();
            return true;
        }

        public bool AddTrainingProgram(int dribblingDrillId, int difficultyId)
        {
            if (!_context.DribblingDrills.Where(dd => dd.Id == dribblingDrillId).Any() ||
                !_context.Difficulties.Where(d => d.Id == difficultyId).Any()) {
                return false;
            }

            _context.TrainingPrograms.Add(new TrainingProgram()
            {
                DifficultyId = difficultyId,
                DribblingDrillId = dribblingDrillId
            });

            _context.SaveChanges();
            return true;
        }

        public List<Difficulty> GetDifficulties()
        {
            if (_context.Difficulties.Count() == 0)
                return new List<Difficulty>();
            return _context.Difficulties.ToList();
        }

        public List<Tuple<string,int>> GetTrainingPrograms()
        {
            if (!_context.TrainingPrograms.Any())
                return new List<Tuple<string, int>>();
            var dribblingDrills = _context.DribblingDrills;
            var difficulties = _context.Difficulties;
            return 
                _context
                .TrainingPrograms
                .Select(x => new Tuple<string, int>(
                    dribblingDrills
                        .Where(drill => drill.Id == x.DribblingDrillId)
                        .First()
                        .Name,
                    difficulties
                        .Where(difficulty => difficulty.Id == x.DifficultyId)
                        .First()
                        .DifficultyLevel
                    )
                ).ToList();
        }

        public List<DribblingDrill> GetDribblingDrills()
        {
            if (_context.DribblingDrills.Count() == 0)
                return new List<DribblingDrill>();
            return _context.DribblingDrills.ToList();
        }

        public bool DeleteDribblingDrill(int drillId)
        {
            if (!_context.DribblingDrills.Where(d => d.Id == drillId).Any())
                return false;
            var drill = _context.DribblingDrills.Where(d => d.Id == drillId).First();
            _context.DribblingDrills.Remove(drill);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteTrainingProgram(int drillId, int difficultyId)
        {
            if (!_context
                .TrainingPrograms
                .Where(d => d.DribblingDrillId == drillId
                            && d.DifficultyId == difficultyId)
                .Any())
            {
                return false;
            }
            var trainingProgram =
                _context
                .TrainingPrograms
                .Where(d => d.DribblingDrillId == drillId
                    && d.DifficultyId == difficultyId)
                .First();

            _context.TrainingPrograms.Remove(trainingProgram);
            _context.SaveChanges();
            return true;
        }

        public bool DeleteDifficulty(int difficultyId)
        {
            if (!_context.Difficulties.Where(d => d.Id == difficultyId).Any())
            {
                return false;
            }
            var difficulty = _context.Difficulties.Where(d => d.Id == difficultyId).First();
            _context.Difficulties.Remove(difficulty);
            _context.SaveChanges();
            return true;
        }
       
    }
}
