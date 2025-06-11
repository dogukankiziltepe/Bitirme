using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Entities.User
{
    public class UserMailCode:BaseEntity
    {
        public string Code { get; set; }
        public int Type { get; set; }
        public string UserId { get; set; }
    }
}
