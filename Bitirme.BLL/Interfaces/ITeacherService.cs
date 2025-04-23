using Bitirme.DAL.Entities.User;
using System.Collections.Generic;

namespace Bitirme.BLL.Interfaces
{
    public interface ITeacherService
    {
        IEnumerable<Teacher> GetAll();
        Teacher GetById(string id);
        void Add(Teacher teacher);
        void Update(Teacher teacher);
        void Delete(string id);
    }
}