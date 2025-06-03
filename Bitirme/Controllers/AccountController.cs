using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Services;
using Bitirme.DAL.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bitirme.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IConfiguration _configuration;
        public AccountController(IAccountService accountService,IConfiguration configuration)
        {
            _accountService = accountService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var result = _accountService.Login(request.EMail, request.Password);
            if (result == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = JWTTokenHelpercs.GenerateJwtToken(result.Id, result.UserType.ToString(), _configuration);
            return Ok(new { Token = token, User = result });
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] SignUpRequest request)
        {
            if(string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Email))
            {
                return BadRequest("Please Fill All Fields!");
            }
            var result = _accountService.SignUp(request.Password, request.Email,request.Name, request.UserType);
            if (!result)
            {
                return BadRequest("Sign up failed. Please try again.");
            }

            return Ok("Sign up successful.");
        }

        [HttpGet("verify-email")] 
        public IActionResult VerifyEmail(string userId) 
        { 
            var result = _accountService.VerifyEmail(userId); 
            if (!result) 
            { 
                return BadRequest("Email verification failed."); 
            } 
            return Ok("Email verified successfully."); 
        }

        [HttpGet("GetStudentInfo/{studentId}")]
        public IActionResult GetStudentInfo(string studentId)
        {
            var result = _accountService.GetStudentInfo(studentId);
            if (result == null)
            {
                return BadRequest("Didn't find student classes please try again later.");
            }
            return Ok(result);
        }

        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(ResetPasswordRequest request)
        {
            var result = _accountService.ResetPassword(request.StudentId,request.OldPassword,request.NewPassword);
            if (!result)
            {
                return BadRequest("Old password is wrong!");
            }
            return Ok();

        }
    }

    public class LoginRequest
    {
        public string EMail { get; set; }
        public string Password { get; set; }
    }
    public class ResetPasswordRequest
    {
        public string StudentId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class SignUpRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }
    }
}