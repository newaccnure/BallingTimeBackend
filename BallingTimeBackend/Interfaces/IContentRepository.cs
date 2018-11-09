using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BallingTimeBackend.Models;
using BallingTimeBackend.Data_for_frontend;

namespace BallingTimeBackend.Interfaces
{
    public interface IContentRepository
    {
        bool AddDifficulty(int secondsForExercise, int difficultyLevel);
        List<Difficulty> GetAllDifficulties();
        bool AddDribblingDrill(string name, string description, string videoReference);
        List<DribblingDrill> GetAllDribblingDrills();
    }
}
