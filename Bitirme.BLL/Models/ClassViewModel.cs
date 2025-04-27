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
        public int Capacity { get; set; } // Maximum number of students allowed in the class
        public string TeacherId { get; set; }
        public string Name { get; set; }
        public TeacherViewModel Teacher { get; set; }
        public int? CurrentStudentCount { get; set; } // Number of students currently in the class
        public List<LessonViewModel>? Lessons { get; set; }

    }
}
