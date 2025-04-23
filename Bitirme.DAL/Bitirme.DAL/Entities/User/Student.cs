using Bitirme.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.User
{
    public class Student:BaseUser
    {
        public virtual ICollection<Class> Classes { get; set; } = new List<Class>();
        public bool IsEmailActivated { get; set; } = false;
    }
}
