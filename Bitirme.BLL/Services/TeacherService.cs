using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Abstracts.Users;
using Bitirme.DAL.Entities.User;
using System.Collections.Generic;

namespace Bitirme.BLL.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _teacherRepository;

        public TeacherService(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        public IEnumerable<Teacher> GetAll()
        {
            return _teacherRepository.GetAll();
        }

        public Teacher GetById(string id)
        {
            return _teacherRepository.GetById(id);
        }

        public void Add(Teacher teacher)
        {
            _teacherRepository.Add(teacher);
            _teacherRepository.SaveChanges();
        }

        public void Update(Teacher teacher)
        {
            _teacherRepository.Update(teacher);
            _teacherRepository.SaveChanges();
        }

        public void Delete(string id)
        {
            _teacherRepository.Delete(id);
            _teacherRepository.SaveChanges();
        }
    }
}