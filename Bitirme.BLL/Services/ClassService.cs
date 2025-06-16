using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Entities.Courses;
using System.Collections.Generic;
using System.Linq;
using System;
using Bitirme.DAL.Entities.User;
using Bitirme.BLL.Models;
using Bitirme.DAL.Entities;
using Bitirme.DAL.Abstracts.Users;
using System.Reflection.Metadata.Ecma335;

namespace Bitirme.BLL.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ILessonService _lessonService;
        private readonly ITeacherService _teacherService;
        private readonly ILessonStudentRepository _lessonStudentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IClassStudentRepository _classStudentRepository;

        public ClassService(IClassRepository classRepository, IStudentRepository studentRepository,
            ILessonService lessonService, ITeacherService teacherService,
            ILessonStudentRepository lessonStudentRepository, IQuestionRepository questionRepository, IClassStudentRepository classStudentRepository)
        {
            _classRepository = classRepository;
            _studentRepository = studentRepository;
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
                Level = x.Level,
                Name = x.Name,
            });
        }

        public ClassViewModel GetById(string id)
        {
            var dbClass = _classRepository.GetById(id);
            var lessons = _lessonService.GetLessonsWithClassId(id);
            return new ClassViewModel
            {
                Id = dbClass.Id,
                Level = dbClass.Level,
                Name = dbClass.Name,
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
            var classviewModels = new List<ClassViewModel>();
            var classes  = _classRepository.FindWithInclude(c => c.Students.Any(s => s.Student.Id == studentId), c => c.Students, c => c.Teacher, c => c.Lessons, c=> c.Course).ToList();

            foreach (var classViewModel in classes)
            {
                var studentClass = _classStudentRepository.FindWithInclude(x => x.Student.Id == studentId && x.Class.Id == classViewModel.Id).FirstOrDefault();
                if(studentClass == null)
                {
                    continue;
                }
                if(studentClass.RecordStatus != RecordStatus.Completed)
                {
                    var lessons = _lessonService.GetLessonsWithClassId(classViewModel.Id);
                    var lessonViewModels = new List<LessonViewModel>();
                    foreach (var lesson in lessons)
                    {
                        var questions = _lessonService.GetLessonQuestions(lesson.Id);
                        if(questions.Count() > 0)
                        {
                            lessonViewModels.Add(new LessonViewModel
                            {
                                ClassId = classViewModel.Id,
                                Content = lesson.Content,
                                Id = lesson.Id,
                                Order = lesson.Order,
                                IsCompleted = _lessonStudentRepository.FindWithInclude(classViewModel => classViewModel.LessonId == lesson.Id && classViewModel.StudentId == studentId && classViewModel.Status == RecordStatus.Completed).FirstOrDefault() != null ? true : false
                            });
                        }
                    }
                    classviewModels.Add(new ClassViewModel
                    {
                        Id = classViewModel.Id,
                        Lessons = lessonViewModels,
                        Level = classViewModel.Level,
                        Name = classViewModel.Name,
                        CourseId = classViewModel.Course.Id,
                        CourseName = classViewModel.Course.Name,
                    });
                }
            }
            return classviewModels;
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

        public IEnumerable<ClassViewModel> GetAllClassesWithDetails()
        {
            return _classRepository.GetAll().Select(c => new ClassViewModel
            {
                Id = c.Id,
                Name = c.Name,

            });
        }


        public Student GetStudentById(string studentId)
        {
            return _studentRepository.GetById(studentId);
        }

        public IEnumerable<ClassViewModel> GetClassesByCourseId(string courseId)
        {
            try
            {
                var classes = _classRepository.FindWithInclude(x => x.Course.Id == courseId, x => x.Students,x => x.Teacher);
                return classes.Select(x => new ClassViewModel
                {
                    Id = x.Id,
                    Level = x.Level,
                    Name = x.Name,
                }).ToList();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ClassViewModel> GetClassCourseIdAndStudentId(string courseId, string studentId)
        {
            var dbclasses = _classRepository.FindWithInclude(x => x.Course.Id == courseId, x => x.Lessons, x=> x.Course).ToList();
            var modelClasses = new List<ClassViewModel>();
            foreach (var item in dbclasses)
            {
                var lessonIds = item.Lessons.Select(x => x.Id).ToList();
                var studentLessons = _lessonStudentRepository.FindWithInclude(x => lessonIds.Any(a => a == x.LessonId) && x.StudentId == studentId).ToList();
                var lessons = new List<LessonViewModel>();
                var dbLessons = _lessonService.GetLessonsWithClassId(item.Id);
                foreach (var les in dbLessons)
                {
                    var questions = _lessonService.GetLessonQuestions(les.Id);
                    if (questions.Count < 1)
                    {
                        continue;
                    }
                    lessons.Add(new LessonViewModel
                    {
                        ClassId = item.Id,
                        Content = les.Content,
                        Id = les.Id,
                        Order = les.Order,
                        IsCompleted = _lessonStudentRepository.FindWithInclude(x => x.LessonId == les.Id && x.StudentId == studentId && x.Status == RecordStatus.Completed).FirstOrDefault() != null ? true : false,
                        QuestionCount = questions.Count,
                    });
                }
                modelClasses.Add(new ClassViewModel
                {
                    Id = item.Id,
                    Level = item.Level,
                    Name = item.Name,
                    CourseName = item.Course.Name,
                    CourseId = item.CourseId,
                    Lessons = lessons,
                    StudentStatus = studentLessons.Count() == 0 ? StudentClassProgress.NotStarted : studentLessons.Count() == item.Lessons.Count() ? StudentClassProgress.Completed : StudentClassProgress.Continue
                });
            }
            return modelClasses;
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

        public List<ClassViewModel> GetTeacherClasses(string teacherId,string courseId)
        {
            var classes = _classRepository.FindWithInclude(c => c.Teacher.Id == teacherId && c.CourseId == courseId, c => c.Students, c => c.Teacher, c => c.Lessons, c => c.Course).ToList();
            var modelClasses = new List<ClassViewModel>();
            foreach (var item in classes)
            {
                var lessonIds = item.Lessons.Select(x => x.Id).ToList();
                var lessons = new List<LessonViewModel>();
                var dbLessons = _lessonService.GetLessonsWithClassId(item.Id);
                foreach (var les in dbLessons)
                {
                    var questions = _lessonService.GetLessonQuestions(les.Id);
         
                    lessons.Add(new LessonViewModel
                    {
                        ClassId = item.Id,
                        Content = les.Content,
                        Id = les.Id,
                        Order = les.Order,
                        QuestionCount = questions.Count,
                    });
                }
                modelClasses.Add(new ClassViewModel
                {
                    Id = item.Id,
                    Level = item.Level,
                    Name = item.Name,
                    CourseName = item.Course.Name,
                    CourseId = item.CourseId,
                    Lessons = lessons,
                });
            }
            return modelClasses;
        }
    }
}