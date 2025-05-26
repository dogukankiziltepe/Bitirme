using Bitirme.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.BLL.Models
{
    public class CourseRegister
    {
        public string CourseId { get; set; }
        public Level Level { get; set; }
    }

    public class AllCourseRegister
    {
        public string StudentId { get; set; }
        public List<CourseRegister> CourseRegisters { get; set; }
    }
}
