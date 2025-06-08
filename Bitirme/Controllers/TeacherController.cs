using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Services;
using Bitirme.DAL.Entities.User;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bitirme.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly IClassService _classService;

        public TeacherController(ITeacherService teacherService,IClassService classService)
        {
            _teacherService = teacherService;
            _classService = classService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Teacher>> GetAll()
        {
            return Ok(_teacherService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Teacher> GetById(string id)
        {
            var teacher = _teacherService.GetById(id);
            if (teacher == null)
            {
                return NotFound();
            }
            return Ok(teacher);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Teacher teacher)
        {
            _teacherService.Add(teacher);
            return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, teacher);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Teacher teacher)
        {
            if (id != teacher.Id)
            {
                return BadRequest();
            }
            _teacherService.Update(teacher);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _teacherService.Delete(id);
            return NoContent();
        }

        [HttpGet("GetTeacherClasses/{teacherId}")]
        public IActionResult GetTeacherClasses(string teacherId)
        {
            var result = _classService.GetTeacherClasses(teacherId);
            return Ok(result);
        }
    }
}