using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.BLL.Models
{
    public class LessonViewModel
    {
        public string? Id { get; set; }
        public int? Order { get; set; }
        public string ClassId { get; set; }
        public string Content { get; set; }
        public bool? IsCompleted { get; set; }
        public List<StudentViewModel>? Students { get; set; }
        public List<QuestionViewModel>? Questions { get; set; }
    }
}
