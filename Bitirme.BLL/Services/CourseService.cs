using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Entities.Courses;
using System.Collections.Generic;

namespace Bitirme.BLL.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public IEnumerable<Course> GetAll()
        {
            return _courseRepository.GetAll();
        }

        public Course GetById(string id)
        {
            return _courseRepository.GetById(id);
        }

        public void Add(Course course)
        {
            _courseRepository.Add(course);
            _courseRepository.SaveChanges();
        }

        public void Update(Course course)
        {
            _courseRepository.Update(course);
            _courseRepository.SaveChanges();
        }

        public void Delete(string id)
        {
            _courseRepository.Delete(id);
            _courseRepository.SaveChanges();
        }
    }
}