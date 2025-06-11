using Bitirme.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.User
{
    public class Teacher:BaseUser
    {
        public bool IsMainTeacher { get; set; }
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public bool IsEmailActivated { get; set; }
    }
}
