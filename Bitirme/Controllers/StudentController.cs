using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
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
        public IActionResult GetAll()
        {
            return Ok(_studentService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            var student = _studentService.GetById(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost("AllCourseRegister")]
        public IActionResult AllCourseRegister(AllCourseRegister allCourseRegister)
        {
            _studentService.AllCourseRegister(allCourseRegister);
            return Ok();
        }
    }
}