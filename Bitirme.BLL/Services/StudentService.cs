using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Abstracts.Users;
using Bitirme.DAL.Entities.User;
using System.Collections.Generic;

namespace Bitirme.BLL.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        public IEnumerable<Student> GetAll()
        {
            return _studentRepository.GetAll();
        }

        public Student GetById(string id)
        {
            return _studentRepository.GetById(id);
        }

        public void Add(Student student)
        {
            _studentRepository.Add(student);
            _studentRepository.SaveChanges();
        }

        public void Update(Student student)
        {
            _studentRepository.Update(student);
            _studentRepository.SaveChanges();
        }

        public void Delete(string id)
        {
            _studentRepository.Delete(id);
            _studentRepository.SaveChanges();
        }
    }
}