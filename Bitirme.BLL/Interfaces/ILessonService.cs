using Bitirme.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.BLL.Interfaces
{
    public interface ILessonService
    {
        public bool CreateLesson(LessonViewModel lessonViewModel);
        public List<LessonViewModel> GetLessonsWithClassId(string classId);
        public bool CompleteLesson(string studentId, string lessonId);
        public bool AddLessonQuestions(string lessonId, List<QuestionViewModel> questionViewModels);
    }
}
