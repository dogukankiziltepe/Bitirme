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
    public class LessonStudentRepository : Repository<LessonStudent>, ILessonStudentRepository
    {
        private readonly BitirmeDbContext _context;
        public LessonStudentRepository(BitirmeDbContext bitirmeDbContext) : base(bitirmeDbContext)
        {
            _context = bitirmeDbContext;
        }


        public bool Any(string studentId, string LessonId)
        {
            return _context.LessonStudents.Any(x => x.LessonId == LessonId && x.StudentId == studentId);
        }
    }
}
