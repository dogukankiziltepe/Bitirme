using Bitirme.DAL.Entities.Medias;
using System.Collections.Generic;

namespace Bitirme.BLL.Interfaces
{
    public interface IClassMediaService
    {
        IEnumerable<ClassMedia> GetAll();
        ClassMedia GetById(string id);
        void Add(ClassMedia classMedia);
        void Update(ClassMedia classMedia);
        void Delete(string id);
    }
}