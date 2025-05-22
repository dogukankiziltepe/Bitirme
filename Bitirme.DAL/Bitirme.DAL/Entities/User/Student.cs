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
        public virtual ICollection<ClassStudent> Classes { get; set; } = new List<ClassStudent>();
        public bool IsEmailActivated { get; set; } = false;
    }
}
