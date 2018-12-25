using System.Collections.Generic;
using BallingTimeBackend.Models;

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
