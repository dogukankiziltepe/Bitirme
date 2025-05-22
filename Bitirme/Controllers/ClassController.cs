using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Bitirme.BLL.Services;
using Bitirme.DAL.Entities.Courses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bitirme.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
        }
        /// <summary>
        /// Tüm classlarý getirir.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_classService.GetAllClassesWithDetails());
        }
        /// <summary>
        /// Id'ye göre class getirme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var classEntity = _classService.GetById(id);
            if (classEntity == null)
            {
                return NotFound();
            }
            return Ok(classEntity);
        }
        /// <summary>
        /// Yeni class oluþturma (Id parametresi boþ gönderilmeli)
        /// </summary>
        /// <param name="classEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Add([FromBody] ClassDTO classEntity)
        {
            _classService.Add(classEntity);
            return CreatedAtAction(nameof(GetById), new { id = classEntity.Id }, classEntity);
        }
        /// <summary>
        /// Var olan class'ý gönderme
        /// </summary>
        /// <param name="id"></param>
        /// <param name="classEntity"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] ClassDTO classEntity)
        {
            if (id != classEntity.Id)
            {
                return BadRequest();
            }
            _classService.Update(classEntity);
            return NoContent();
        }
        /// <summary>
        /// Class'ý silme
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _classService.Delete(id);
            return NoContent();
        }
        /// <summary>
        /// Student'ý Class'a ekleme.
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpPost("{classId}/students/{studentId}")]
        public IActionResult AddStudentToClass(string classId, string studentId)
        {
            var classEntity = _classService.GetById(classId);
            if (classEntity == null)
            {
                return NotFound($"Class with ID {classId} not found.");
            }

            var student = _classService.GetStudentById(studentId); // Assuming this method exists in the service
            if (student == null)
            {
                return NotFound($"Student with ID {studentId} not found.");
            }

            _classService.AddStudentToClass(classId, studentId);
            return NoContent();
        }

        /// <summary>
        /// studentId ile Classlarý getirme
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("students/{studentId}/classes")]
        public ActionResult<IEnumerable<Class>> GetClassesByStudentId(string studentId)
        {
            var classes = _classService.GetClassesByStudentId(studentId);
            if (classes == null || !classes.Any())
            {
                return NotFound($"No classes found for student with ID {studentId}.");
            }
            return Ok(classes);
        }

        /// <summary>
        /// studentId ve courseId ile Classý getirme
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("{courseId}/{studentId}/classes")]
        public ActionResult<IEnumerable<Class>> GetClassesByStudentId(string courseId,string studentId)
        {
            var classes = _classService.GetClassCourseIdAndStudentId(courseId,studentId);
            if (classes == null)
            {
                return NotFound($"No classes found for student with ID {studentId}.");
            }
            return Ok(classes);
        }

        /// <summary>
        /// courseId ile Classlarý getirme
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("{courseId}/classes")]
        public ActionResult<IEnumerable<Class>> GetClassesByCourseId(string courseId, string studentId)
        {
            var classes = _classService.GetClassesByCourseId(courseId);
            if (classes == null)
            {
                return NotFound($"No classes found for student with ID {studentId}.");
            }
            return Ok(classes);
        }

        [HttpGet("details")]
        public ActionResult<IEnumerable<ClassViewModel>> GetAllClassesWithDetails()
        {
            return Ok(_classService.GetAllClassesWithDetails());
        }

        /// <summary>
        /// Class'ý bitirme sýnavýný ekleme
        /// </summary>
        /// <param name="questions"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost("AddClassExam/{classId}")]
        public IActionResult AddClassExam(List<QuestionViewModel> questions, string classId)
        {
            var result = _classService.AddClassExam(questions, classId);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }

        /// <summary>
        /// Class'ý bitirip bir üst seviyeye çýkmasýný saðlayan endpoint
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        [HttpGet("CompleteClass/{classId}/{studentId}")]
        public IActionResult CompletedClass(string classId, string studentId)
        {
            var result = _classService.CompletedClass(classId, studentId);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}