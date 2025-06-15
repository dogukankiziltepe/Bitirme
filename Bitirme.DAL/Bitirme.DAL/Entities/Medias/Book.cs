using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.Medias
{
    public class Book:BaseEntity
    {
        public string CoverName { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
    }
}
