using Bitirme.DAL.Abstracts.Users;
using Bitirme.DAL.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Concretes.Users
{
    public class StudentRepository:Repository<Student>, IStudentRepository
    {
        public StudentRepository(BitirmeDbContext bitirmeDbContext) : base(bitirmeDbContext)
        {
        }
    }
}
