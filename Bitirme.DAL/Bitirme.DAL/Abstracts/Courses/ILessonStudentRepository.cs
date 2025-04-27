using Bitirme.DAL.Entities.Courses;
using Bitirme.DAL.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Abstracts.Courses
{
    public interface ILessonStudentRepository : IRepository<LessonStudent>
    {
        public bool Any(string studentId, string LessonId);
    }
}
