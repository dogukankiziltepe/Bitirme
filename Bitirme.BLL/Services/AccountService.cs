using Bitirme.BLL.Interfaces;
using Bitirme.DAL;
using Bitirme.DAL.Entities.User;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Bitirme.BLL.Services
{
    public enum UserType
    {
        Teacher,
        Student
    }

    public class AccountViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }
    }

    public class AccountService:IAccountService
    {
        private readonly BitirmeDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountService(BitirmeDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public AccountViewModel Login(string username, string password)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Username == username && t.Password == password);
            if (teacher != null)
            {
                return new AccountViewModel
                {
                    Id = teacher.Id,
                    Name = teacher.Name,
                    UserType = UserType.Teacher
                };
            }

            var student = _context.Students.FirstOrDefault(s => s.Username == username && s.Password == password);
            if (student != null)
            {
                return new AccountViewModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    UserType = UserType.Student
                };
            }

            return null; // Login failed
        }

        public bool SignUp(string username, string password, string email,string name,string surname,DateTime birthDate, UserType userType)
        {
            // Check if the user already exists
            if (_context.Teachers.Any(t => t.Username == username) || _context.Students.Any(s => s.Username == username))
            {
                return false; // User already exists
            }

            if (userType == UserType.Teacher)
            {
                // Create a new teacher
                var newTeacher = new Teacher
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = username,
                    Password = password, // In a real-world scenario, hash the password before saving
                    Email = email,
                    Name = name,
                    Surname = surname,
                    BirthDate = birthDate,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _context.Teachers.Add(newTeacher);
            }
            else if (userType == UserType.Student)
            {
                // Create a new student
                var newStudent = new Student
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = username,
                    Password = password, // In a real-world scenario, hash the password before saving
                    Email = email,
                    Name = name,
                    BirthDate = birthDate,
                    Surname = surname,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _context.Students.Add(newStudent);
            }

            _context.SaveChanges();


            return true; // Sign-up successful
        }

        public bool VerifyEmail(string userId)
        {
            throw new NotImplementedException();
        }
    }
}