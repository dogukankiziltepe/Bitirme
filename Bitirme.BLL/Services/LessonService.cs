using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Abstracts.Users;
using Bitirme.DAL.Concretes.Courses;
using Bitirme.DAL.Entities;
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
                ListeningSentence = x.ListeningSentence,
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

        public bool CompleteLesson(string studentId, string lessonId, bool next = true)
        {
            try
            {
                var student = _studentRepository.GetById(studentId);
                if(student == null)
                {
                    throw new Exception("This Student Not Found!");
                }
                var lesson = _lessonRepository.FindWithInclude(x => x.Id == lessonId,x => x.Class).FirstOrDefault();
                if(lesson == null)
                {
                    throw new Exception("This Lesson Not Found!");
                }
                _lessonStudentRepository.Add(new LessonStudent
                {
                    CreatedDate = DateTime.UtcNow,
                    Lesson = lesson,
                    Student = student,
                    Status = DAL.Entities.RecordStatus.Completed,
                });
                _lessonStudentRepository.SaveChanges();
                var allLessons = _lessonRepository.FindWithInclude(x => x.Class.Id == lesson.Class.Id, x => x.Class).ToList();

                
                if(!allLessons.Any(x => x.Order > lesson.Order))
                {
                    var dbclassStudent = _classStudentRepository.FindWithInclude(x => x.Class.Id == lesson.Class.Id && x.Student.Id == studentId, x=> x.Student, x => x.Class).FirstOrDefault();
                    var dbclassStudents = _classStudentRepository.FindWithInclude(x => x.Class.Id == lesson.Class.Id && x.Student.Id == studentId, x => x.Student, x => x.Class).ToList();
                    if (next)
                    {
                        var isTheEnd = Enum.IsDefined(typeof(RecordStatus), (int)dbclassStudent.Class.Level + 1);
                        if (isTheEnd)
                        {
                            return true;
                        }
                        var newLevel = (int)dbclassStudent.Class.Level + 1;
                        var dbclass = _classRepository.GetById(dbclassStudent.Class.Id);
                        var newClass = _classRepository.FindWithInclude(x => x.Level == (Level)newLevel && x.Course.Id == dbclass.CourseId).FirstOrDefault();
                        AddStudentToClass(newClass.Id, studentId);
                    }
                    dbclassStudent.RecordStatus = DAL.Entities.RecordStatus.Completed;
                    _classStudentRepository.Update(dbclassStudent);
                    _classStudentRepository.SaveChanges();


                }
                return true;
            }
            catch (Exception ex)
            {

                return true;
            }
        }

        public bool CreateLesson(LessonViewModel lessonViewModel)
        {
            try
            {
                var dbclass = _classRepository.GetById(lessonViewModel.ClassId);
                var dbLessons = _lessonRepository.FindWithInclude(x => x.Class.Id == lessonViewModel.ClassId).OrderByDescending(x => x.Order).ToList();
                var order = dbLessons.FirstOrDefault().Order;
                _lessonRepository.Add(new Lesson
                {
                    CreatedDate = DateTime.UtcNow,
                    Content = lessonViewModel.Content,
                    Order = order+1,
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
            var lessons = _lessonRepository.FindWithInclude(x => x.Class.Id == classId, x => x.Class, x => x.Class.Students,  x => x.CompletedStudent).ToList();
            var classStudents = _classStudentRepository.FindWithInclude(x => x.Class.Id == classId, x => x.Student,x=> x.Class).ToList();
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
        
        public List<QuestionViewModel> GetLessonQuestions(string lessonId)
        {
            var questions = _questionRepository.FindWithInclude(x => x.LessonId == lessonId).ToList();
            return questions.Select(x => new QuestionViewModel
            {
                QuestionString = x.QuestionString,
                AnswerFour = x.AnswerFour,
                AnswerOne = x.AnswerOne,
                AnswerTwo = x.AnswerTwo,
                AnswerThree = x.AnswerThree,
                CorrectAnswer = x.CorrectAnswer,
                Id = x.Id,
                ListeningSentence = x.ListeningSentence
            }).ToList();
        }

        public void AddStudentToClass(string classId, string studentId)
        {
            var classEntity = _classRepository.GetById(classId);
            if (classEntity == null)
            {
                throw new Exception($"Class with ID {classId} not found.");
            }

            if (classEntity.Students.Count >= classEntity.Capacity)
            {
                throw new Exception($"Class with ID {classId} is already at full capacity.");
            }

            var student = _studentRepository.GetById(studentId);
            if (student == null)
            {
                throw new Exception($"Student with ID {studentId} not found.");
            }

            classEntity.Students.Add(new ClassStudent
            {
                CreatedDate = DateTime.UtcNow,
                RecordStatus = DAL.Entities.RecordStatus.Continue,
                Student = student,
                Class = classEntity,
            });
            _classRepository.Update(classEntity);
            _classRepository.SaveChanges();
        }

        public bool AddLesson(LessonViewModel model)
        {
            try
            {
                _lessonRepository.Add(new Lesson
                {
                    Class = _classRepository.GetById(model.ClassId),
                    Content = model.Content,
                    Order = model.Order,
                    CreatedDate = DateTime.Now,
                });
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool DeleteLessonQuestion(string questionId)
        {
            _questionRepository.Delete(questionId);
            _questionRepository.SaveChanges();
            return true;
        }

        public bool UpdateLessonQuestion(QuestionViewModel model)
        {
           var dbQuestion = _questionRepository.GetById(model.Id);
            dbQuestion.AnswerOne = model.AnswerOne;
            dbQuestion.AnswerTwo = model.AnswerTwo;
            dbQuestion.AnswerThree = model.AnswerThree;
            dbQuestion.AnswerFour = model.AnswerFour;
            dbQuestion.QuestionString = model.QuestionString;
            dbQuestion.CorrectAnswer = model.CorrectAnswer;
            dbQuestion.ListeningSentence = model.ListeningSentence;
            _questionRepository.Update(dbQuestion);
            _questionRepository.SaveChanges();
            return true;
        }

        public bool DeleteLesson(string lessonId)
        {
            _lessonRepository.Delete(lessonId);
            _lessonRepository.SaveChanges();
            return true;
        }
    }
}
