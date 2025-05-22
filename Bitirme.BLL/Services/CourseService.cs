using Bitirme.BLL.Interfaces;
using Bitirme.BLL.Models;
using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Entities.Courses;
using System.Collections.Generic;

namespace Bitirme.BLL.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IQuestionRepository _questionRepository;

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

        public bool AddCourseLevelExam(List<QuestionViewModel> questionViewModels, string courseId)
        {
            var questions = questionViewModels.Select(x => new Question
            {
                QuestionString = x.QuestionString,
                AnswerOne = x.AnswerOne,
                AnswerTwo = x.AnswerTwo,
                AnswerThree = x.AnswerThree,
                AnswerFour = x.AnswerFour,
                Course = _courseRepository.GetById(courseId),
                CourseId = courseId
            });
            return true;
        }

        public List<QuestionViewModel> GetCourseLevelExam(string courseId)
        {
            return _questionRepository.FindWithInclude(x => x.CourseId == courseId, x => x.QuestionMedia).Select(x => new QuestionViewModel
            {
                Id = x.Id,
                QuestionString = x.QuestionString,
                AnswerOne = x.AnswerOne,
                AnswerTwo = x.AnswerTwo,
                AnswerThree = x.AnswerThree,
                AnswerFour = x.AnswerFour,
                CorrectAnswer = x.CorrectAnswer,
                QuestionMedia = new MediaViewModel
                {
                    FileName = x.QuestionMedia != null ? x.QuestionMedia.MediaName : "",
                    Id = x.Id,
                },
                QuestionMediaId = x.QuestionMediaId
            }).ToList();

        }
    }
}