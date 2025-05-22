using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Entities.Courses;
using System.Collections.Generic;
using System.Linq;
using System;
using Bitirme.DAL.Entities.User;
using Bitirme.BLL.Models;

namespace Bitirme.BLL.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IStudentService _studentService;
        private readonly ILessonService _lessonService;
        private readonly ITeacherService _teacherService;
        private readonly ILessonStudentRepository _lessonStudentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IClassStudentRepository _classStudentRepository;

        public ClassService(IClassRepository classRepository, IStudentService studentService,
            ILessonService lessonService, ITeacherService teacherService,
            ILessonStudentRepository lessonStudentRepository, IQuestionRepository questionRepository, IClassStudentRepository classStudentRepository)
        {
            _classRepository = classRepository;
            _studentService = studentService;
            _lessonService = lessonService;
            _teacherService = teacherService;
            _lessonStudentRepository = lessonStudentRepository;
            _questionRepository = questionRepository;
            _classStudentRepository = classStudentRepository;
        }

        public IEnumerable<ClassViewModel> GetAll()
        {
            return _classRepository.GetAll().Select(x => new ClassViewModel
            {
                Id = x.Id,
                Capacity = x.Capacity,
                Level = x.Level,
                Name = x.Name,
                TeacherId = x.TeacherId,
                CurrentStudentCount = x.Students.Count
            });
        }

        public ClassViewModel GetById(string id)
        {
            var dbClass = _classRepository.GetById(id);
            var lessons = _lessonService.GetLessonsWithClassId(id);
            return new ClassViewModel
            {
                Id = dbClass.Id,
                Capacity = dbClass.Capacity,
                Level = dbClass.Level,
                Name = dbClass.Name,
                TeacherId = dbClass.TeacherId,
                CurrentStudentCount = dbClass.Students.Count,
                Lessons = lessons.Select(x => new LessonViewModel
                {
                    Students = x.Students,
                    Content = x.Content,
                    Order = x.Order,
                    Id = x.Id,
                    ClassId = x.ClassId,

                }).ToList(),
            };
        }

        public void Add(ClassDTO classEntity)
        {
            var teacher = _teacherService.GetById(classEntity.TeacherId);
            _classRepository.Add(new Class
            {
                Capacity = classEntity.Capacity,
                Level=classEntity.Level,
                Name=classEntity.Name,
                TeacherId=classEntity.TeacherId,
                Teacher = teacher,
                CreatedDate = DateTime.UtcNow,
            });
            _classRepository.SaveChanges();
        }

        public void Update(ClassDTO classEntity)
        {
            var dbClass = _classRepository.GetById(classEntity.Id);
            dbClass.Level = classEntity.Level;
            dbClass.Capacity = classEntity.Capacity;
            _classRepository.Update(dbClass);
            _classRepository.SaveChanges();
        }

        public void Delete(string id)
        {
            _classRepository.Delete(id);
            _classRepository.SaveChanges();
        }

        public IEnumerable<ClassViewModel> GetClassesByStudentId(string studentId)
        {
            return _classRepository.FindWithInclude(c => c.Students.Any(s => s.Student.Id == studentId), c => c.Students, c => c.Teacher, c => c.Lessons).Select(x => new ClassViewModel
            {
                Id = x.Id,
                Teacher = new TeacherViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                },
                Capacity = x.Capacity,
                CurrentStudentCount = x.Students.Count(),
                Lessons = x.Lessons.Select(a => new LessonViewModel
                {
                    ClassId = x.Id,
                    Content = a.Content,
                    Id = a.Id,
                    Order = a.Order,
                    IsCompleted = _lessonStudentRepository.Any(studentId,a.Id)
                }).ToList(),
                Level = x.Level,
                Name = x.Name,
                TeacherId = x.TeacherId
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

            var student = _studentService.GetById(studentId); // Assuming this method exists
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

        public IEnumerable<ClassViewModel> GetAllClassesWithDetails()
        {
            return _classRepository.GetAll().Select(c => new ClassViewModel
            {
                Id = c.Id,
                Name = c.Name,
                Capacity = c.Capacity,
                CurrentStudentCount = c.Students.Count
            });
        }


        public Student GetStudentById(string studentId)
        {
            return _studentService.GetById(studentId);
        }

        public IEnumerable<ClassViewModel> GetClassesByCourseId(string courseId)
        {
            try
            {
                var classes = _classRepository.FindWithInclude(x => x.Course.Id == courseId, x => x.Students,x => x.Teacher);
                return classes.Select(x => new ClassViewModel
                {
                    Id = x.Id,
                    CurrentStudentCount = x.Students.Count,
                    Capacity = x.Capacity,
                    Level = x.Level,
                    Name = x.Name,
                    Teacher = new TeacherViewModel
                    {
                        Id = x.Teacher.Id,
                        Name = x.Teacher.Name,
                    }
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ClassViewModel> GetClassCourseIdAndStudentId(string courseId, string studentId)
        {
            var dbclass = _classRepository.FindWithInclude(x => x.Course.Id == courseId && x.Students.Any(s => s.Student.Id == studentId), x => x.Students, x => x.Teacher, x => x.Lessons).ToList();
            return dbclass.Select(x => new ClassViewModel
            {
                Id = x.Id,
                CurrentStudentCount = x.Students.Count,
                Capacity = x.Capacity,
                Level = x.Level,
                Name = x.Name,
                Teacher = new TeacherViewModel
                {
                    Id = x.Teacher.Id,
                    Name = x.Teacher.Name,
                },
                Lessons = x.Lessons.Select(a => new LessonViewModel
                {
                    ClassId = x.Id,
                    Content = a.Content,
                    Id = a.Id,
                    Order = a.Order,
                    IsCompleted = _lessonStudentRepository.Any(studentId, a.Id)
                }).ToList()
            }).ToList();
            
        }

        public bool AddClassExam(List<QuestionViewModel> questionViewModels, string classId)
        {
            var questions = questionViewModels.Select(x => new Question
            {
                QuestionString = x.QuestionString,
                AnswerOne = x.AnswerOne,
                AnswerTwo = x.AnswerTwo,
                AnswerThree = x.AnswerThree,
                AnswerFour = x.AnswerFour,
                CorrectAnswer = x.CorrectAnswer,
                Class = _classRepository.GetById(classId),
                ClassId = classId
            });
            _questionRepository.AddRange(questions);
            _questionRepository.SaveChanges();
            return true;
        }

        public List<QuestionViewModel> GetClassExam(string classId)
        {
            var questionViewModels = _questionRepository.FindWithInclude(x => x.ClassId == classId).Select(x => new QuestionViewModel
            {
                QuestionString = x.QuestionString,
                AnswerOne = x.AnswerOne,
                AnswerTwo = x.AnswerTwo,
                AnswerThree = x.AnswerThree,
                AnswerFour = x.AnswerFour,
                CorrectAnswer = x.CorrectAnswer,
                Id = x.Id
            }).ToList();
            return questionViewModels;
        }

        public bool CompletedClass(string classId, string studentId)
        {
            var dbclass = _classRepository.GetById(classId);
            var classStudent =  _classStudentRepository.FindWithInclude(x => x.Class.Id == classId && x.Student.Id == studentId).FirstOrDefault();
            classStudent.RecordStatus = DAL.Entities.RecordStatus.Completed;
            _classStudentRepository.Update(classStudent);
            _classStudentRepository.SaveChanges();
            var nextSeviye =(Level)((int)dbclass.Level + 1);
            var nextClass = _classRepository.FindWithInclude(x => x.Level == nextSeviye && x.Course.Id == dbclass.CourseId).FirstOrDefault();
            AddStudentToClass(nextClass.Id,studentId);
            _classRepository.SaveChanges();
            return true;
        }
    }
}