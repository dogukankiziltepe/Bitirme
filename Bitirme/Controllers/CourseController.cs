using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Entities.Courses;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Bitirme.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Course>> GetAll()
        {
            return Ok(_courseService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Course> GetById(string id)
        {
            var course = _courseService.GetById(id);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost]
        public IActionResult Add([FromBody] Course course)
        {
            _courseService.Add(course);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }
            _courseService.Update(course);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            _courseService.Delete(id);
            return NoContent();
        }
    }
}