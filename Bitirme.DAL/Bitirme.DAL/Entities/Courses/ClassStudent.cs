using Bitirme.DAL.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.Courses
{
    public class ClassStudent:BaseEntity
    {
        public Class Class { get; set; }
        public Student Student { get; set; }
        public RecordStatus RecordStatus { get; set; }
    }
}
