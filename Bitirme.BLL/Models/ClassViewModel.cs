using Bitirme.DAL.Entities;
using Bitirme.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.BLL.Models
{
    public class ClassViewModel
    {
        public string? Id { get; set; }
        public Level Level { get; set; }
        public string CourseName { get; set; }
        public string CourseId { get; set; }
        public string Name { get; set; }
        public int CompletedLessonCount { get; set; }
        public StudentClassProgress StudentStatus { get; set; }
        public List<LessonViewModel>? Lessons { get; set; }
        public int LessonCount { get;  set; }
        public int MyProperty { get; set; }
    }
}
