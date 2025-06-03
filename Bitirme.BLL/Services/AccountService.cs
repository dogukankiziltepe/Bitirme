using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
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
        public string EMail { get; internal set; }
        public List<ClassViewModel> Classes { get; set; }
    }

    public class AccountService:IAccountService
    {
        private readonly BitirmeDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IClassService _classService;

        public AccountService(BitirmeDbContext context, IConfiguration configuration, IClassService classService)
        {
            _context = context;
            _configuration = configuration;
            _classService = classService;
        }

        public AccountViewModel Login(string email, string password)
        {
            var teacher = _context.Teachers.FirstOrDefault(t => t.Email == email && t.Password == password);
            if (teacher != null)
            {
                return new AccountViewModel
                {
                    Id = teacher.Id,
                    Name = teacher.Name,
                    UserType = UserType.Teacher,
                    EMail = teacher.Email,
                };
            }

            var student = _context.Students.FirstOrDefault(s => s.Email == email && s.Password == password);
            if (student != null)
            {
                var classes = _classService.GetClassesByStudentId(student.Id);

                return new AccountViewModel
                {
                    Id = student.Id,
                    Name = student.Name,
                    UserType = UserType.Student,
                    EMail = student.Email,
                    Classes = classes.ToList()
                };
            }

            return null; // Login failed
        }

        public bool SignUp(string password, string email,string name, UserType userType)
        {
            // Check if the user already exists
            if (_context.Teachers.Any(t => t.Email == email))
            {
                return false; // User already exists
            }

            if (userType == UserType.Teacher)
            {
                // Create a new teacher
                var newTeacher = new Teacher
                {
                    Id = Guid.NewGuid().ToString(),
                    Password = password, // In a real-world scenario, hash the password before saving
                    Email = email,
                    Name = name,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                _context.Teachers.Add(newTeacher);
            }
            else if (userType == UserType.Student)
            {
                var random = new Random();
                // Create a new student
                var newStudent = new Student
                {
                    Id = Guid.NewGuid().ToString(),
                    Password = password, // In a real-world scenario, hash the password before saving
                    Email = email,
                    Name = name,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    ProfilePicture = random.Next(0, 4).ToString() + ".png"
                };

                _context.Students.Add(newStudent);
            }

            _context.SaveChanges();


            return true; // Sign-up successful
        }

        public IEnumerable<ClassViewModel> GetStudentInfo (string studentId)
        {
            var classes = _classService.GetClassesByStudentId(studentId);
            return classes;
        }

        public bool VerifyEmail(string userId)
        {
            throw new NotImplementedException();
        }

        public bool ResetPassword(string studentId, string oldPassword, string newPassword)
        {
            var user = _context.Students.FirstOrDefault(x => x.Id == studentId);
            if (user == null)
            {
                return false;
            }
            if(user.Password != oldPassword)
            {
                return false;
            }
            user.Password = newPassword;
            _context.Students.Update(user);
            _context.SaveChanges();
            return true;
        }
    }
}