using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BallingTimeBackend.Models;

namespace BallingTimeBackend.Interfaces
{
    public interface IContentRepository
    {
        bool AddDifficulty(int secondsForExercise, int difficultyLevel);
        List<Difficulty> GetAllDifficulties();
        bool AddDribblingExercise(string name, string description, string videoReference);
        List<DribblingDrill> GetAllDribblingExercises();
        List<Tuple<DribblingDrill, double, double>> GetUserProgressById(int userId);
        Tuple<List<DribblingDrill>, Difficulty> GetFullTrainingProgramById(int userId);
    }
}
