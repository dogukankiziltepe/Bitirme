using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Abstracts.Users;
using Bitirme.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.BLL.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILessonStudentRepository _lessonStudentRepository;

        public LessonService(ILessonRepository lessonRepository, 
            IClassRepository classRepository,
            IStudentRepository studentRepository,
            ILessonStudentRepository lessonStudentRepository)
        {
            _lessonRepository = lessonRepository;
            _classRepository = classRepository;
            _studentRepository = studentRepository;
            _lessonStudentRepository = lessonStudentRepository;
        }

        public bool CompleteLesson(string studentId, string lessonId)
        {
            try
            {
                var student = _studentRepository.GetById(studentId);
                if(student == null)
                {
                    throw new Exception("This Student Not Found!");
                }
                var lesson = _lessonRepository.GetById(lessonId);
                if(lesson == null)
                {
                    throw new Exception("This Lesson Not Found!");
                }
                _lessonStudentRepository.Add(new LessonStudent
                {
                    CreatedDate = DateTime.Now,
                    Lesson = lesson,
                    Student = student
                });
                _lessonStudentRepository.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool CreateLesson(LessonViewModel lessonViewModel)
        {
            try
            {
                var dbclass = _classRepository.GetById(lessonViewModel.ClassId);
                _lessonRepository.Add(new Lesson
                {
                    CreatedDate = DateTime.Now,
                    Content = lessonViewModel.Content,
                    Order = lessonViewModel.Order,
                    Class = dbclass,
                });
                _lessonRepository.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        public List<LessonViewModel> GetLessonsWithClassId(string classId)
        {
            var lessons = _lessonRepository.FindWithInclude(x => x.Class.Id == classId, x => x.Class, x => x.Class.Students, x => x.CompletedStudent);
            return lessons.Select(x => new LessonViewModel
            {
                Id = x.Id,
                ClassId = x.Class.Id,
                Content = x.Content,
                Order = (int)x.Order,
                Students = x.Class.Students.Select(a => new StudentViewModel
                {
                    Id = a.Id,
                    Name = a.Name,
                }).ToList()
            }).ToList();
        }
    }
}
