using Bitirme.DAL.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.Courses
{
    public class LessonStudent:BaseEntity
    {
        public string StudentId { get; set; }
        public Student Student { get; set; }
        public string LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public RecordStatus Status { get; set; } = RecordStatus.Unknown;
    }
}
