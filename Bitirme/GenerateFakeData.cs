using Bitirme.DAL.Entities.Courses;
using Bitirme.DAL.Entities.User;
using Bitirme.DAL;
using Microsoft.Extensions.DependencyInjection;
using Bogus;
using Bogus.DataSets;
using System.Linq;

namespace Bitirme
{
    public class GenerateFakeData
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<BitirmeDbContext>();

            if (!context.Teachers.Any() && !context.Students.Any() && !context.Courses.Any())
            {
                // Türkçe veriler oluşturmak için Bogus'un kültür ayarını Türkçe olarak belirleyin

                var teacher = new Teacher
                {
                    Name = "Master Teacher",
                    Password = "123456aA!",
                    IsMainTeacher = true,
                    CreatedDate = DateTime.UtcNow,
                    Email = "master_teacher@mail.com"
                };

                var studentFaker = new Faker<Student>("tr")
                    .RuleFor(s => s.Name, f => f.Name.FullName())
                    .RuleFor(s => s.Username, f => f.Name.FirstName())
                    .RuleFor(s => s.Password, "123456")
                    .RuleFor(s => s.Email, f => f.Internet.Email())
                    .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(t => t.Surname, f => f.Name.LastName())
                    .RuleFor(t => t.Address, f => f.Address.FullAddress())
                    .RuleFor(t => t.BirthDate, f => f.Date.Between(DateTime.UtcNow.AddYears(-14), DateTime.UtcNow.AddYears(-50)));


                var students = studentFaker.Generate(20);
                var courses = new List<Course>
                {
                    new Course
                    {
                        Name = "Speaking",
                        Description = "Speaking Course",
                        CourseType = CourseType.Speaking,
                        CreatedDate = DateTime.UtcNow,
                    },
                    new Course
                    {
                        Name = "Listening",
                        Description = "Listening Course",
                        CourseType = CourseType.Listening,
                        CreatedDate = DateTime.UtcNow,
                    },
                    new Course
                    {
                        Name = "Reading",
                        Description = "Reading Course",
                        CourseType = CourseType.Reading,
                        CreatedDate = DateTime.UtcNow,
                    },
                    new Course
                    {
                        Name = "Writing",
                        Description = "Writing Course",
                        CourseType = CourseType.Writing,
                        CreatedDate = DateTime.UtcNow,
                    },

                };

                context.Teachers.Add(teacher);
                context.Students.AddRange(students);
                context.Courses.AddRange(courses);

                context.SaveChanges();
            }
        }
    }
}
