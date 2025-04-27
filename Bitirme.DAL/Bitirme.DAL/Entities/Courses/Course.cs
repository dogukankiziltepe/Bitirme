using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.Courses
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; } // in hours
        public Level Level { get; set; }
        public CourseType CourseType { get; set; }
        public List<Class> Classes { get; set; } = new List<Class>();
    }
}
