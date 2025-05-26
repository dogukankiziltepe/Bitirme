using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Abstracts.Users;
using Bitirme.DAL.Entities.User;
using System.Collections.Generic;

namespace Bitirme.BLL.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IClassService _classService;
        private readonly ILessonService _lessonService;

        public StudentService(IStudentRepository studentRepository,
            IClassService classService,ILessonService lessonService)
        {
            _studentRepository = studentRepository;
            _classService = classService;
            _lessonService = lessonService;
        }

        public IEnumerable<Student> GetAll()
        {
            return _studentRepository.GetAll();
        }

        public Student GetById(string id)
        {
            return _studentRepository.GetById(id);
        }

        public void Add(Student student)
        {
            _studentRepository.Add(student);
            _studentRepository.SaveChanges();
        }

        public void Update(Student student)
        {
            _studentRepository.Update(student);
            _studentRepository.SaveChanges();
        }

        public void Delete(string id)
        {
            _studentRepository.Delete(id);
            _studentRepository.SaveChanges();
        }

        public void AllCourseRegister(AllCourseRegister allCourseRegister)
        {
            try
            {
                foreach (var item in allCourseRegister.CourseRegisters)
                {
                    var classes = _classService.GetClassesByCourseId(item.CourseId);
                    var selectedClass = classes.Where(x => x.Level == item.Level).FirstOrDefault();
                    var completedClasses = classes.Where(x => (int)x.Level < (int)item.Level).ToList();
                    _classService.AddStudentToClass(selectedClass.Id,allCourseRegister.StudentId);
                    foreach (var completedClass in completedClasses)
                    {
                        _classService.AddStudentToClass(completedClass.Id, allCourseRegister.StudentId);
                        var lessons = _lessonService.GetLessonsWithClassId(completedClass.Id);
                        foreach (var lesson in lessons)
                        {
                            _lessonService.CompleteLesson(allCourseRegister.StudentId, lesson.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}