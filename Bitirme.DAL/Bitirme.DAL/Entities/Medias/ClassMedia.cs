using Bitirme.DAL.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.Medias
{
    public class ClassMedia:BaseEntity
    {
        public string ClassId { get; set; }
        public virtual Class Class { get; set; }
        public string MediaName { get; set; }
        public string MediaType { get; set; } // Video, Audio, Document, etc.
        public string MediaUrl { get; set; } // URL or path to the media file
    }
}

