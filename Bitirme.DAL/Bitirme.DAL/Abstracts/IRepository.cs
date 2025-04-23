using Bitirme.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Abstracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        public IQueryable<T> GetAll();
        public T GetById(string id);
        public void Add(T entity);
        public void Update(T entity);
        public void Delete(string id);
        public void SaveChanges();
        public void AddRange(IEnumerable<T> entities);

    }
}
