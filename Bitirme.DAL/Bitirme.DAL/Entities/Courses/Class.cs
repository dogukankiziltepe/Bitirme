using Bitirme.DAL.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.Courses
{
    public class Class: BaseEntity
    {
        public string Name { get; set; }
        public int? Capacity { get; set; } // Maximum number of students allowed in the class
        public Level Level { get; set; }
        public string TeacherId { get; set; }
        public virtual Teacher Teacher { get; set; }
        public string CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<ClassStudent> Students { get; set; } = new List<ClassStudent>();
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public virtual List<Question> LessonQuestions { get; set; } = new List<Question>();

    }
}
