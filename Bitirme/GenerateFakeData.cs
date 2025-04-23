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
                var teacherFaker = new Faker<Teacher>("tr")
                    .RuleFor(s => s.Username,f => f.Name.FirstName())
                    .RuleFor(s => s.Password,"123456")
                    .RuleFor(s => s.Name, f => f.Name.FullName())
                    .RuleFor(s => s.Email, f => f.Internet.Email())
                    .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(t => t.Surname, f => f.Name.LastName())
                    .RuleFor(t => t.Address, f => f.Address.FullAddress())
                    .RuleFor(t => t.BirthDate, f => f.Date.Between(DateTime.UtcNow.AddYears(-18), DateTime.UtcNow.AddYears(-50)));

                var studentFaker = new Faker<Student>("tr")
                    .RuleFor(s => s.Name, f => f.Name.FullName())
                    .RuleFor(s => s.Username, f => f.Name.FirstName())
                    .RuleFor(s => s.Password, "123456")
                    .RuleFor(s => s.Email, f => f.Internet.Email())
                    .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
                    .RuleFor(t => t.Surname, f => f.Name.LastName())
                    .RuleFor(t => t.Address, f => f.Address.FullAddress())
                    .RuleFor(t => t.BirthDate, f => f.Date.Between(DateTime.UtcNow.AddYears(-14), DateTime.UtcNow.AddYears(-50)));


                var courseFaker = new Faker<Course>("tr")
                    .RuleFor(c => c.Name, f => f.Company.CatchPhrase())
                    .RuleFor(c => c.Description, f => f.Company.CatchPhrase())
                    .RuleFor(c => c.Duration, f => f.Random.Number(20, 40))
                    .RuleFor(c => c.Level, f => f.PickRandom<Level>());

                var classFaker = new Faker<Class>("tr")
                    .RuleFor(cl => cl.Name, f => f.Commerce.Department())
                    .RuleFor(cl => cl.Capacity, f => f.Random.Number(20, 50))
                    .RuleFor(cl => cl.Level, f => f.PickRandom<Level>());

                var teachers = teacherFaker.Generate(5);
                var students = studentFaker.Generate(20);
                var courses = courseFaker.Generate(5);

                foreach (var course in courses)
                {
                    var classes = classFaker.Generate(5);
                    foreach (var classItem in classes)
                    {
                        classItem.Teacher = teachers[new Random().Next(teachers.Count)]; // Rastgele bir öğretmen ata
                    }
                    course.Classes = classes;
                }

                context.Teachers.AddRange(teachers);
                context.Students.AddRange(students);
                context.Courses.AddRange(courses);

                context.SaveChanges();
            }
        }
    }
}
