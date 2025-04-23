using Bitirme.BLL.Services;
using Bitirme.DAL.Entities.Courses;
using Bitirme.DAL.Entities.User;
using System.Collections.Generic;

namespace Bitirme.BLL.Interfaces
{
    public interface IClassService
    {
        IEnumerable<Class> GetAll();
        Class GetById(string id);
        void Add(Class classEntity);
        void Update(Class classEntity);
        void Delete(string id);
        IEnumerable<Class> GetClassesByStudentId(string studentId);
        void AddStudentToClass(string classId, string studentId);
        IEnumerable<ClassViewModel> GetAllClassesWithDetails();
        Student GetStudentById(string studentId);
    }
}