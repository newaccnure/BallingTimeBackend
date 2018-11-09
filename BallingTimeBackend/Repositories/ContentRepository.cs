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

    }
}
