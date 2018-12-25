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
    }
}