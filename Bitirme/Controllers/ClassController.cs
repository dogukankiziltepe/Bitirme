using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Bitirme.BLL.Services;
using Bitirme.DAL.Entities.Courses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Bitirme.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ClassController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassController(IClassService classService)
        {
            _classService = classService;
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

        [HttpPost("CreateClass")]
        public IActionResult CreateClass(ClassDTO model)
        {
            _classService.Add(model);
            return Ok();
        }

    }
}