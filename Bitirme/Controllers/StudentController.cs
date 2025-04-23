using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Entities.User;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bitirme.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAll()
        {
            return Ok(_studentService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Student> GetById(string id)
        {
            var student = _studentService.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Student student)
        {
            _studentService.Add(student);
            return CreatedAtAction(nameof(GetById), new { id = student.Id }, student);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }
            _studentService.Update(student);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _studentService.Delete(id);
            return NoContent();
        }
    }
}