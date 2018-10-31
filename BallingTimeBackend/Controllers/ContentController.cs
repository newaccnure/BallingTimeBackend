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
        [Route("getAllDribblingExercises")]
        public JsonResult GetAllDribblingExercises()
        {
            return Json(_contentRepository.GetAllDribblingExercises());
        }

        [HttpPost]
        [Route("addDribblingExercise")]
        public JsonResult AddDribblingExercise(string name, string description, string videoReference)
        {
            return Json(_contentRepository.AddDribblingExercise(name, description, videoReference));
        }

        [HttpPost]
        [Route("getFullTrainingProgramById")]
        public JsonResult GetFullTrainingProgramById(int userId)
        {
            return Json(_contentRepository.GetFullTrainingProgramById(userId));
        }

        [HttpPost]
        [Route("getUserProgressById")]
        public JsonResult GetUserProgressById(int userId)
        {
            return Json(_contentRepository.GetUserProgressById(userId));
        }
    }
}