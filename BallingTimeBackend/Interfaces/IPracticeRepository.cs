using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BallingTimeBackend.Data_for_frontend;

namespace BallingTimeBackend.Interfaces
{
    public interface IPracticeRepository
    {
        List<UserStats> GetUserStatsById(int userId);
        List<Drill_Info> GetFullTrainingProgramById(int userId);
        bool CheckDayOfPractice(int userId);
        bool AddDrillToCompleted(int userId, int drillId, 
            double averageSpeed, double averageAccuracy, double repeatitionsPerSecond);
        DrillStats GetDrillStatsById(int userId, int drillId);
    }
}
