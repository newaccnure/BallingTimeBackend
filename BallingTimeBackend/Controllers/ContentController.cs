using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BallingTimeBackend.Interfaces;

namespace BallingTimeBackend.Controllers
{
    [Route("api/[controller]")]
    public class ContentController : Controller
    {
        public IContentRepository _contentRepository;
        public ContentController(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        [HttpPost]
        [Route("addDifficulty")]
        public JsonResult AddDifficulty(int secondsForExercise, int difficultyLevel)
        {
            return Json(_contentRepository.AddDifficulty(secondsForExercise, difficultyLevel));
        }

        [HttpGet]
        [Route("getAllDifficulties")]
        public JsonResult GetAllDifficulties()
        {
            return Json(_contentRepository.GetAllDifficulties());
        }

        [HttpGet]
        [Route("getAllDribblingDrills")]
        public JsonResult GetAllDribblingDrills()
        {
            return Json(_contentRepository.GetAllDribblingDrills());
        }

        [HttpPost]
        [Route("addDribblingDrill")]
        public JsonResult AddDribblingDrill(string name, string description, string videoReference)
        {
            return Json(_contentRepository.AddDribblingDrill(name, description, videoReference));
        }

        [HttpPost]
        [Route("getFullTrainingProgramById")]
        public JsonResult GetFullTrainingProgramById(int userId)
        {
            return Json(_contentRepository.GetFullTrainingProgramById(userId));
        }

        [HttpPost]
        [Route("getUserStatsById")]
        public JsonResult GetUserStatsById(int userId)
        {
            return Json(_contentRepository.GetUserStatsById(userId));
        }

        [HttpPost]
        [Route("checkDayOfPractice")]
        public JsonResult CheckDayOfPractice(int userId)
        {
            return Json(_contentRepository.CheckDayOfPractice(userId));
        }
    }
}