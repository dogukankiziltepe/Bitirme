using Bitirme.BLL.Interfaces;
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

        [HttpGet]
        public ActionResult<IEnumerable<ClassViewModel>> GetAll()
        {
            return Ok(_classService.GetAllClassesWithDetails());
        }

        [HttpGet("{id}")]
        public ActionResult<Class> GetById(string id)
        {
            var classEntity = _classService.GetById(id);
            if (classEntity == null)
            {
                return NotFound();
            }
            return Ok(classEntity);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Class classEntity)
        {
            _classService.Add(classEntity);
            return CreatedAtAction(nameof(GetById), new { id = classEntity.Id }, classEntity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Class classEntity)
        {
            if (id != classEntity.Id)
            {
                return BadRequest();
            }
            _classService.Update(classEntity);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _classService.Delete(id);
            return NoContent();
        }

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

        [HttpGet("details")]
        public ActionResult<IEnumerable<ClassViewModel>> GetAllClassesWithDetails()
        {
            return Ok(_classService.GetAllClassesWithDetails());
        }
    }
}