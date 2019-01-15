using Microsoft.AspNetCore.Mvc;
using BallingTimeBackend.Interfaces;
using Microsoft.AspNetCore.Cors;

namespace BallingTimeBackend.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class ContentController : Controller
    {
        public IContentRepository _contentRepository;
        public ContentController(IContentRepository contentRepository)
        {
            _contentRepository = contentRepository;
        }

        [HttpPut]
        [Route("addDifficulty")]
        public JsonResult AddDifficulty(int secondsForExercise, int difficultyLevel)
        {
            return Json(_contentRepository.AddDifficulty(secondsForExercise, difficultyLevel));
        }

        [HttpPut]
        [Route("addDribblingDrill")]
        public JsonResult AddDribblingDrill(string name, string description, string videoReference)
        {
            return Json(_contentRepository.AddDribblingDrill(name, description, videoReference));
        }

        [HttpPut]
        [Route("addTrainingProgram")]
        public JsonResult AddTrainingProgram(int dribblingDrillId, int difficultyId)
        {
            return Json(_contentRepository.AddTrainingProgram(dribblingDrillId, difficultyId));
        }

        [HttpGet]
        [Route("getDifficulties")]
        public JsonResult GetDifficulties()
        {
            return Json(_contentRepository.GetDifficulties());
        }

        [HttpGet]
        [Route("getDribblingDrills")]
        public JsonResult GetDribblingDrills()
        {
            return Json(_contentRepository.GetDribblingDrills());
        }

        [HttpGet]
        [Route("getTrainingPrograms")]
        public JsonResult GetTrainingPrograms()
        {
            return Json(_contentRepository.GetTrainingPrograms());
        }

        [HttpDelete]
        [Route("deleteTrainingProgram")]
        public JsonResult DeleteTrainingProgram(int drillId, int difficultyId)
        {
            return Json(_contentRepository.DeleteTrainingProgram(drillId, difficultyId));
        }

        [HttpDelete]
        [Route("deleteDribblingDrill")]
        public JsonResult DeleteDribblingDrill(int drillId)
        {
            return Json(_contentRepository.DeleteDribblingDrill(drillId));
        }

        [HttpDelete]
        [Route("deleteDifficulty")]
        public JsonResult DeleteDifficulty(int difficultyId)
        {
            return Json(_contentRepository.DeleteDifficulty(difficultyId));
        }
    }
}