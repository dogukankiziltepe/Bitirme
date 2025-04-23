using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Entities.Courses;
using System.Collections.Generic;
using System.Linq;
using System;
using Bitirme.DAL.Entities.User;

namespace Bitirme.BLL.Services
{
    public class ClassService : IClassService
    {
        private readonly IClassRepository _classRepository;
        private readonly IStudentService _studentService;

        public ClassService(IClassRepository classRepository, IStudentService studentService)
        {
            _classRepository = classRepository;
            _studentService = studentService;
        }

        public IEnumerable<Class> GetAll()
        {
            return _classRepository.GetAll();
        }

        public Class GetById(string id)
        {
            return _classRepository.GetById(id);
        }

        public void Add(Class classEntity)
        {
            _classRepository.Add(classEntity);
            _classRepository.SaveChanges();
        }

        public void Update(Class classEntity)
        {
            _classRepository.Update(classEntity);
            _classRepository.SaveChanges();
        }

        public void Delete(string id)
        {
            _classRepository.Delete(id);
            _classRepository.SaveChanges();
        }

        public IEnumerable<Class> GetClassesByStudentId(string studentId)
        {
            return _classRepository.GetAll().Where(c => c.Students.Any(s => s.Id == studentId));
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

            classEntity.Students.Add(student);
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

        public IEnumerable<ClassViewModel> GetClassesByCourseId(string courseId)
        {
            return _classRepository.GetAll()
                .Where(c => c.CourseId == courseId)
                .Select(c => new ClassViewModel
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
    }

    public class ClassViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; } // Maximum number of students allowed in the class
        public int CurrentStudentCount { get; set; } // Number of students currently in the class
    }
}