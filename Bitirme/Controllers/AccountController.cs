using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Services;
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
    }

    public class LoginRequest
    {
        public string EMail { get; set; }
        public string Password { get; set; }
    }

    public class SignUpRequest
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }
    }
}