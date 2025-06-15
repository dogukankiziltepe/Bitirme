using Bitirme.DAL.Abstracts.Medias;
using Bitirme.DAL.Entities.Medias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Concretes.Medias
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(BitirmeDbContext bitirmeDbContext) : base(bitirmeDbContext)
        {
        }
    }
}
