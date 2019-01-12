using System;
using System.Collections.Generic;
using BallingTimeBackend.Models;

namespace BallingTimeBackend.Interfaces
{
    public interface IContentRepository
    {
        bool AddDifficulty(int secondsForExercise, int difficultyLevel);
        bool AddTrainingProgram(int dribblingDrillId, int difficultyId);
        bool AddDribblingDrill(string name, string description, string videoReference);
        List<Difficulty> GetDifficulties();
        List<Tuple<string, int>> GetTrainingPrograms();
        List<DribblingDrill> GetDribblingDrills();
        bool DeleteDribblingDrill(int drillId);
        bool DeleteTrainingProgram(int drillId, int difficultyId);
        bool DeleteDifficulty(int difficultyId);
        bool DeleteUserProgress();
    }
}
