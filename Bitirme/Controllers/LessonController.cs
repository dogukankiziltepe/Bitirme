using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Bitirme.DAL.Entities.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bitirme.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
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
            var result = _lessonService.CreateLesson(lesson);
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

        [HttpGet("DeleteLessonQuestion/{questionId}")]
        public IActionResult DeleteLessonQuestion(string questionId)
        {
            var result = _lessonService.DeleteLessonQuestion(questionId);
            if(result)
                return Ok(result);
            return BadRequest();
        }

        [HttpPost("UpdateLessonQuestion")]
        public IActionResult UpdateLessonQuestion(QuestionViewModel model)
        {
            var result = _lessonService.UpdateLessonQuestion(model);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
                
        }

        [HttpGet("DeleteLesson/{lessonId}")]
        public IActionResult DeleteLesson(string lessonId)
        {
            var result = _lessonService.DeleteLesson(lessonId);
            if (result)
                return Ok(result);
            return BadRequest();
        }

    }
}
