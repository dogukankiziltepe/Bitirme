using Bitirme.DAL.Abstracts.Courses;
using Bitirme.DAL.Entities.Courses;
using Bitirme.DAL.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Concretes.Courses
{
    public class LessonRepository : Repository<Lesson>, ILessonRepository
    {
        public LessonRepository(BitirmeDbContext bitirmeDbContext) : base(bitirmeDbContext)
        {
        }
    }
}
