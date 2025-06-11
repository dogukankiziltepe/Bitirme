using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Bitirme.DAL;
using Bitirme.DAL.Abstracts.Users;
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
        public bool IsEmailActivated { get; internal set; }
    }

    public class AccountService:IAccountService
    {
        private readonly BitirmeDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IClassService _classService;
        private readonly IUserMailCodeRepository _userMailCodeRepository;
        public AccountService(BitirmeDbContext context, IConfiguration configuration, IClassService classService,IUserMailCodeRepository userMailCodeRepository)
        {
            _context = context;
            _configuration = configuration;
            _classService = classService;
            _userMailCodeRepository = userMailCodeRepository;
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
                    IsEmailActivated = teacher.IsEmailActivated,
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
                    Classes = classes.ToList(),
                    IsEmailActivated = student.IsEmailActivated,
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
                var random = new Random();
                var code = random.Next(100000, 999999);
                // Create a new teacher

                var newTeacher = new Teacher
                {
                    Id = Guid.NewGuid().ToString(),
                    Password = password, // In a real-world scenario, hash the password before saving
                    Email = email,
                    Name = name,
                    CreatedDate = DateTime.UtcNow,
                    IsEmailActivated = false,
                    UpdatedDate = DateTime.UtcNow,
                };

                _userMailCodeRepository.Add(new UserMailCode
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = code.ToString(),
                    Type = 0,
                    UserId = newTeacher.Id
                });
                var body = "Registration Code: " + code;
                EmailService.SendEmail(newTeacher.Email,"Verification Code",body);


                _context.Teachers.Add(newTeacher);
            }
            else if (userType == UserType.Student)
            {
                var random = new Random();
                var code = random.Next(100000, 999999);
                // Create a new student
                var newStudent = new Student
                {
                    Id = Guid.NewGuid().ToString(),
                    Password = password, // In a real-world scenario, hash the password before saving
                    Email = email,
                    Name = name,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow,
                    IsEmailActivated = false,
                    ProfilePicture = random.Next(0, 4).ToString() + ".png"
                };

                _userMailCodeRepository.Add(new UserMailCode
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = code.ToString(),
                    Type = 0,
                    UserId = newStudent.Id
                });
                var body = "Registration Code: " + code;
                EmailService.SendEmail(newStudent.Email, "Verification Code", body);

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

        public bool VerifyEmail(string userId,string code)
        {
            var userCode = _userMailCodeRepository.FindWithInclude(x => x.UserId == userId).FirstOrDefault();
            if (userCode == null)
            {
                return false;
            }
            if(userCode.Code == code)
            {
                var student = _context.Students.FirstOrDefault(x => x.Id == userId);
                if (student == null)
                {
                    var teacher = _context.Teachers.FirstOrDefault(x => x.Id == userId);
                    if (teacher == null)
                    {
                        return false;
                    }
                    teacher.IsEmailActivated = true;
                    _context.Teachers.Update(teacher);
                    _context.UserMailCodes.Remove(userCode);
                    _context.SaveChanges();
                }
                else
                {
                    student.IsEmailActivated = true;
                    _context.Students.Update(student);
                    _context.UserMailCodes.Remove(userCode);
                    _context.SaveChanges();
                }
                return true;
            }
            else
            {
                return false;
            }
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

        public string ForgotPasswordMail(string mail)
        {
            var random = new Random();
            var code = random.Next(100000, 999999);
            var student = _context.Students.FirstOrDefault(x => x.Email == mail);
            if (student != null)
            {
                EmailService.SendEmail(student.Email,"Forgot Password Code","Your Code: "+ code);
                _userMailCodeRepository.Add(new UserMailCode
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = code.ToString(),
                    Type = 0,
                    UserId = student.Id
                });
                _context.SaveChanges();
                return student.Id;
            }
            else
            {
                var teacher = _context.Teachers.FirstOrDefault(y => y.Email == mail);
                if (teacher == null)
                {
                    return "";
                }
                EmailService.SendEmail(teacher.Email, "Forgot Password Code", "Your Code: " + code);
                _userMailCodeRepository.Add(new UserMailCode
                {
                    Id = Guid.NewGuid().ToString(),
                    Code = code.ToString(),
                    Type = 0,
                    UserId = teacher.Id 
                });
                _context.SaveChanges();

                return teacher.Id;

            }
        }

        public bool ForgotPasswordCodeControl(string userId,string code)
        {
            var userMailCode = _userMailCodeRepository.FindWithInclude(x => x.UserId == userId).FirstOrDefault();
            if(userMailCode == null)
            {
                return false;
            }
            return userMailCode.Code == code;
        }

        public bool ForgotPasswordChange(string userId, string newPassword)
        {
            var student = _context.Students.FirstOrDefault(x => x.Id == userId);
            if (student == null)
            {
                var teacher = _context.Teachers.FirstOrDefault(x => x.Id == userId);
                if(teacher == null)
                {
                    return false;
                }
                teacher.Password = newPassword;
                _context.Teachers.Update(teacher);
                _context.SaveChanges();
            }
            else
            {
                student.Password = newPassword;
                _context.Students.Update(student);
                _context.SaveChanges();
            }
            return true;
        }
    }
}