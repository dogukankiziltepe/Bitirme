using Bitirme.BLL.Models;
using Bitirme.DAL.Entities.Courses;
using System.Collections.Generic;

namespace Bitirme.BLL.Interfaces
{
    public interface ICourseService
    {
        IEnumerable<Course> GetAll();
        Course GetById(string id);
        void Add(Course course);
        void Update(Course course);
        void Delete(string id);
        bool AddCourseLevelExam(List<QuestionViewModel> questionViewModels,string courseId);
        List<QuestionViewModel> GetCourseLevelExam(string courseId);
    }
}