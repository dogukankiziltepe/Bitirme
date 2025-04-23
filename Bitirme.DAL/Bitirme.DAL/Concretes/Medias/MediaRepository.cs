using Bitirme.DAL.Abstracts.Medias;
using Bitirme.DAL.Entities.Medias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Concretes.Medias
{
    public class MediaRepository : Repository<ClassMedia>, IMediaRepository
    {
        public MediaRepository(BitirmeDbContext bitirmeDbContext) : base(bitirmeDbContext)
        {
        }
    }
}
