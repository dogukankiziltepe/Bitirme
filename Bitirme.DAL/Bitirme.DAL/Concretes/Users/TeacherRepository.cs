using Bitirme.DAL.Abstracts.Users;
using Bitirme.DAL.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Concretes.Users
{
    public class TeacherRepository: Repository<Teacher>, ITeacherRepository
    {
        public TeacherRepository(BitirmeDbContext bitirmeDbContext) : base(bitirmeDbContext)
        {
        }
    }
}
