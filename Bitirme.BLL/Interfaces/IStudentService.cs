using Bitirme.DAL.Entities.User;
using System.Collections.Generic;

namespace Bitirme.BLL.Interfaces
{
    public interface IStudentService
    {
        IEnumerable<Student> GetAll();
        Student GetById(string id);
        void Add(Student student);
        void Update(Student student);
        void Delete(string id);
    }
}