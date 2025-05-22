using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Abstracts.Users;
using Bitirme.DAL.Concretes.Courses;
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
        private readonly IQuestionRepository _questionRepository;
        private readonly IClassStudentRepository _classStudentRepository;

        public LessonService(ILessonRepository lessonRepository, 
            IClassRepository classRepository,
            IStudentRepository studentRepository,
            ILessonStudentRepository lessonStudentRepository,
            IQuestionRepository questionRepository,
            IClassStudentRepository classStudentRepository)
        {
            _lessonRepository = lessonRepository;
            _classRepository = classRepository;
            _studentRepository = studentRepository;
            _lessonStudentRepository = lessonStudentRepository;
            _questionRepository = questionRepository;
            _classStudentRepository = classStudentRepository;
        }

        public bool AddLessonQuestions(string lessonId, List<QuestionViewModel> questionViewModels)
        {
            var questions = questionViewModels.Select(x => new Question
            {
                QuestionString = x.QuestionString,
                AnswerOne = x.AnswerOne,
                AnswerTwo = x.AnswerTwo,
                AnswerThree = x.AnswerThree,
                AnswerFour = x.AnswerFour,
                CorrectAnswer = x.CorrectAnswer,
                Lesson = _lessonRepository.GetById(lessonId),
                LessonId = lessonId
            });
            _questionRepository.AddRange(questions);
            _questionRepository.SaveChanges();
            return true;
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
                    CreatedDate = DateTime.UtcNow,
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
                    CreatedDate = DateTime.UtcNow,
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
            var lessons = _lessonRepository.FindWithInclude(x => x.Class.Id == classId, x => x.Class, x => x.Class.Students,  x => x.CompletedStudent);
            var classStudents = _classStudentRepository.FindWithInclude(x => x.Class.Id == classId, x => x.Student,x=> x.Class);
            return lessons.Select(x => new LessonViewModel
            {
                Id = x.Id,
                ClassId = x.Class.Id,
                Content = x.Content,
                Order = (int)x.Order,
                Questions = x.LessonQuestions.Select(a => new QuestionViewModel
                {
                    QuestionString = a.QuestionString,
                    AnswerOne = a.AnswerOne,
                    AnswerTwo = a.AnswerTwo,
                    AnswerThree = a.AnswerThree,
                    AnswerFour = a.AnswerFour,
                    CorrectAnswer = a.CorrectAnswer,
                    Id = x.Id
                }).ToList(),
                Students = classStudents.Select(a => new StudentViewModel
                {
                    Id = a.Student.Id,
                    Name = a.Student.Name,
                }).ToList()
            }).ToList();
        }
    }
}
