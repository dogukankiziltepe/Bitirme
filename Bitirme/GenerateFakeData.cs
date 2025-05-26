using Bitirme.DAL.Entities.Courses;
using Bitirme.DAL.Entities.User;
using Bitirme.DAL;
using Microsoft.Extensions.DependencyInjection;
using Bogus;
using Bogus.DataSets;
using System.Linq;
using System.Text.Json;

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
                var newClasses = new List<Class>();
                foreach (var course in courses)
                {
                    var classes = new List<Class>
                {
                    new Class
                    {
                        Capacity = 10000,
                        Course = course,
                        CourseId = course.Id,
                        Level = Level.A1,
                        Name = course.Name + " Class",
                        CreatedDate = course.CreatedDate,
                        Teacher = teacher,
                    },
                    new Class
                    {
                        Capacity = 10000,
                        Course = course,
                        CourseId = course.Id,
                        Level = Level.A2,
                        Name = course.Name + " Class",
                        CreatedDate = course.CreatedDate,
                        Teacher = teacher,
                    },
                    new Class
                    {
                        Capacity = 10000,
                        Course = course,
                        CourseId = course.Id,
                        Level = Level.B1,
                        Name = course.Name + " Class",
                        CreatedDate = course.CreatedDate,
                        Teacher = teacher,
                    },
                    new Class
                    {
                        Capacity = 10000,
                        Course = course,
                        CourseId = course.Id,
                        Level = Level.B2,
                        Name = course.Name + " Class",
                        CreatedDate = course.CreatedDate,
                        Teacher = teacher,
                    },
                    new Class
                    {
                        Capacity = 10000,
                        Course = course,
                        CourseId = course.Id,
                        Level = Level.C1,
                        Name = course.Name + " Class",
                        CreatedDate = course.CreatedDate,
                        Teacher = teacher,
                    },
                    new Class
                    {
                        Capacity = 10000,
                        Course = course,
                        CourseId = course.Id,
                        Level = Level.C2,
                        Name = course.Name + " Class",
                        CreatedDate = course.CreatedDate,
                        Teacher = teacher,
                    },
                };
                    newClasses.AddRange(classes);
                   
                }


                context.Teachers.Add(teacher);
                context.Students.AddRange(students);
                context.Courses.AddRange(courses);
                context.Students.AddRange(students);
                context.SaveChanges();
                context.Classes.AddRange(newClasses);
                context.SaveChanges();

                var lessonsRootPath = Path.Combine(Directory.GetCurrentDirectory(), "Lessons");

                foreach (var course in courses)
                {
                    var courseFolder = Path.Combine(lessonsRootPath, course.CourseType.ToString());
                    if (!Directory.Exists(courseFolder)) continue;

                    foreach (var level in Enum.GetValues<Level>())
                    {
                        var levelFolder = Path.Combine(courseFolder, level.ToString());
                        if (!Directory.Exists(levelFolder)) continue;

                        var jsonFiles = Directory.GetFiles(levelFolder, "*.json");
                        var relatedClass = newClasses.FirstOrDefault(c => c.Course.CourseType == course.CourseType && c.Level == level);

                        if (relatedClass == null) continue;

                        foreach (var file in jsonFiles)
                        {
                            string json = File.ReadAllText(file);
                            var jsonLesson = JsonSerializer .Deserialize<JsonLesson>(json);

                            if (jsonLesson != null)
                            {
                                var lesson = new Lesson
                                {
                                    Order = jsonLesson.Order,
                                    Content = jsonLesson.Content,
                                    Class = relatedClass,
                                    CreatedDate = DateTime.UtcNow,
                                    LessonQuestions = jsonLesson.Questions.Select(q => new Question
                                    {
                                        ListeningSentence = q.ListeningSentence,
                                        QuestionString = q.QuestionString,
                                        AnswerOne = q.AnswerOne,
                                        AnswerTwo = q.AnswerTwo,
                                        AnswerThree = q.AnswerThree,
                                        AnswerFour = q.AnswerFour,
                                        CorrectAnswer = q.CorrectAnswer
                                    }).ToList()
                                };

                                context.Lesson.Add(lesson);
                            }
                        }
                    }
                }

                context.SaveChanges();

            }
        }
    }

    public class JsonLesson
    {
        public int Order { get; set; }
        public string Content { get; set; }
        public List<JsonLessonQuestion> Questions { get; set; }
    }

    public class JsonLessonQuestion
    {
        public string ListeningSentence { get; set; }
        public string QuestionString { get; set; }
        public string AnswerOne { get; set; }
        public string AnswerTwo { get; set; }
        public string AnswerThree { get; set; }
        public string AnswerFour { get; set; }
        public string CorrectAnswer { get; set; }
    }
}
