using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.BLL.Models
{
    public class QuestionViewModel
    {
        public string? Id { get; set; }
        public string QuestionString { get; set; }
        public string? AnswerOne { get; set; }
        public string? AnswerTwo { get; set; }
        public string? AnswerThree { get; set; }
        public string? AnswerFour { get; set; }
        public string CorrectAnswer { get; set; }
        public string? QuestionMediaId { get; set; }
        public MediaViewModel? QuestionMedia { get; set; }
    }
}
