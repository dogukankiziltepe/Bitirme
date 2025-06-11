using Bitirme.BLL.Models;
using Bitirme.BLL.Services;
using Bitirme.DAL.Entities.Courses;
using Bitirme.DAL.Entities.User;
using System.Collections.Generic;

namespace Bitirme.BLL.Interfaces
{
    public interface IClassService
    {
        IEnumerable<ClassViewModel> GetAll();
        ClassViewModel GetById(string id);
        void Add(ClassDTO classEntity);
        void Update(ClassDTO classEntity);
        void Delete(string id);
        IEnumerable<ClassViewModel> GetClassesByCourseId(string courseId);
        IEnumerable<ClassViewModel> GetClassesByStudentId(string studentId);
        List<ClassViewModel> GetClassCourseIdAndStudentId(string courseId,string studentId);
        bool AddClassExam(List<QuestionViewModel> questionViewModels, string classId);
        void AddStudentToClass(string classId, string studentId);
        IEnumerable<ClassViewModel> GetAllClassesWithDetails();
        Student GetStudentById(string studentId);
        public List<QuestionViewModel> GetClassExam(string classId);
        bool CompletedClass(string classId, string studentId);
        List<ClassViewModel> GetTeacherClasses(string teacherId,string courseId);
    }
}