using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using BallingTimeBackend.Interfaces;

namespace BallingTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class PracticeController : Controller
    {
        public IPracticeRepository _practiceRepository;
        public PracticeController(IPracticeRepository practiceRepository)
        {
            _practiceRepository = practiceRepository;
        }
        [HttpPost]
        [Route("getFullTrainingProgramById")]
        public JsonResult GetFullTrainingProgramById(int userId)
        {
            return Json(_practiceRepository.GetFullTrainingProgramById(userId));
        }

        [HttpPost]
        [Route("getUserStatsById")]
        public JsonResult GetUserStatsById(int userId)
        {
            return Json(_practiceRepository.GetUserStatsById(userId));
        }

        [HttpPost]
        [Route("checkDayOfPractice")]
        public JsonResult CheckDayOfPractice(int userId)
        {
            return Json(_practiceRepository.CheckDayOfPractice(userId));
        }

        [HttpPost]
        [Route("addDrillToCompleted")]
        public JsonResult AddDrillToCompleted(int userId, int drillId,
            double averageSpeed, double averageAccuracy, double repeatitionsPerSecond)
        {
            return Json(_practiceRepository.AddDrillToCompleted(
                userId, drillId, averageSpeed, averageAccuracy, repeatitionsPerSecond));
        }

        [HttpPost]
        [Route("getDrillStatsById")]
        public JsonResult GetDrillStatsById(int userId, int drillId)
        {
            return Json(_practiceRepository.GetDrillStatsById(userId, drillId));
        }
    }
}