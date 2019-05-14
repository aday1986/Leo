using Leo.Fac;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace Leo.Data.EF
{
    [Service(ServiceType = typeof(IRepository<>))]
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly EFContext db;

        public EFRepository(EFContext db)
        {
            this.db = db;
        }

        public void Add(T entity)
        {
            db.Set<T>().Add(entity);
        }

        public void AddRange(IEnumerable<T> entities)
        {
            db.Set<T>().AddRange(entities);
        }

        public T Get(params object[] keyvalues)
        {
            var entity = db.Set<T>().Find(keyvalues);
            return entity;
        }

        public IEnumerable<T> Query(IEnumerable<Condition> conditions = null)
        {
            return db.Set<T>().AsQueryable<T>().Where(conditions);
        }

        public IEnumerable<T> QueryPage(IEnumerable<Condition> conditions, int index, int pagesize)
        {
            return db.Set<T>().AsQueryable().Where(conditions).Pager(index, pagesize).ToArray();
        }

        public void Remove(T entity)
        {
            db.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            db.Set<T>().RemoveRange(entities);
        }

        public void Update(T entity)
        {
            db.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            db.Set<T>().UpdateRange(entities);
        }

        public int SaveChanges()
        {
            lock (this)
            {
                return db.SaveChanges();
            }
        }

       
    }
}
