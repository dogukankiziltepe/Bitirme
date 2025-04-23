using Bitirme.BLL.Interfaces;
using Bitirme.DAL.Abstracts.Medias;
using Bitirme.DAL.Entities.Medias;
using System.Collections.Generic;

namespace Bitirme.BLL.Services
{
    public class ClassMediaService : IClassMediaService
    {
        private readonly IMediaRepository _classMediaRepository;

        public ClassMediaService(IMediaRepository classMediaRepository)
        {
            _classMediaRepository = classMediaRepository;
        }

        public IEnumerable<ClassMedia> GetAll()
        {
            return _classMediaRepository.GetAll();
        }

        public ClassMedia GetById(string id)
        {
            return _classMediaRepository.GetById(id);
        }

        public void Add(ClassMedia classMedia)
        {
            _classMediaRepository.Add(classMedia);
            _classMediaRepository.SaveChanges();
        }

        public void Update(ClassMedia classMedia)
        {
            _classMediaRepository.Update(classMedia);
            _classMediaRepository.SaveChanges();
        }

        public void Delete(string id)
        {
            _classMediaRepository.Delete(id);
            _classMediaRepository.SaveChanges();
        }
    }
}