using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bitirme.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }
        /// <summary>
        /// Lesson complete edildiğinde kaydedilmesi için gerekli method
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        [HttpGet("CompleteLesson/{studentId}/{lessonId}")]
        public IActionResult CompletedLesson(string studentId,string lessonId)
        {
            var result = _lessonService.CompleteLesson(studentId, lessonId);
            if (result)
                return Ok(result);
            else
                return BadRequest();
        }
        [HttpPost("AddLessonQuestion/{lessonId}")]
        public IActionResult AddLessonQuestions(List<QuestionViewModel> questions, string lessonId)
        {
            var result = _lessonService.AddLessonQuestions(lessonId,questions);
            if (result)
                return Ok(result);
            else
                return BadRequest();
        }
        [HttpPost("AddLesson")]
        public IActionResult AddLesson(LessonViewModel lesson)
        {
            var result = _lessonService.AddLesson(lesson);
            if (result)
                return Ok(result);
            else
                return BadRequest();
        }
        [HttpGet("GetLessonQuestions/{lessonId}")]
        public IActionResult GetLessonQuestions(string lessonId)
        {
            var result = _lessonService.GetLessonQuestions(lessonId);
            if (result != null)
                return Ok(result);
            else
                return BadRequest();
        }

    }
}
