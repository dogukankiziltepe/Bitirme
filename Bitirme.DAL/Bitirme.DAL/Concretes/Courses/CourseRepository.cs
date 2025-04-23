using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Concretes.Courses
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(BitirmeDbContext bitirmeDbContext) : base(bitirmeDbContext)
        {
        }
    }
}
