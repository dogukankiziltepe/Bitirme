using Bitirme.DAL.Abstracts;
using Bitirme.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitirme.DAL.Concretes
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        public BitirmeDbContext _dbContext;
        public DbSet<T> _entities;
        public Repository(BitirmeDbContext bitirmeDbContext)
        {
            _dbContext = bitirmeDbContext;
            _entities = _dbContext.Set<T>();
        }
        public void Add(T entity)
        {
            _entities.Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _entities.AddRange(entities);
        }

        public void Delete(string id)
        {
            var entity = _entities.Find(id);
            _entities.Remove(entity);
        }

        public IQueryable<T> GetAll()
        {
           return _entities.AsQueryable();
        }

        public T GetById(string id)
        {
            return _entities.Find(id);
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _entities.Update(entity);
        }
    }
}
