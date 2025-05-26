using Bitirme.DAL.Entities.Medias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.Courses
{
    public class Question:BaseEntity
    {
        public string QuestionString { get; set; }
        public string? AnswerOne { get; set; }
        public string? AnswerTwo { get; set; }
        public string? AnswerThree { get; set; }
        public string? AnswerFour { get; set; }
        public string CorrectAnswer { get; set; }
        public string? ClassId { get; set; }
        public virtual Class? Class { get; set; }
        public string? LessonId { get; set; }
        public virtual Lesson? Lesson { get; set; }
        public virtual ClassMedia? QuestionMedia { get; set; }
        public string? QuestionMediaId { get; set; }
        public virtual Course? Course { get; set; }
        public string? CourseId { get; set; }
        public string ListeningSentence { get; set; }
    }
}
