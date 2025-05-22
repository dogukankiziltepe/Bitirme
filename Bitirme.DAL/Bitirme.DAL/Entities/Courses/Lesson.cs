using Bitirme.DAL.Entities.Medias;
using Bitirme.DAL.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.Courses
{
    public class Lesson:BaseEntity
    {
        public int? Order { get; set; }
        public string Content { get; set; }
        public int? ClassMediaId { get; set; }
        public virtual ClassMedia? ClassMedia { get; set; }
        public virtual List<Question> LessonQuestions { get; set; } = new List<Question>();
        public virtual ICollection<LessonStudent> CompletedStudent { get; set; }
        public virtual Class Class { get; set; }
    }
}
