using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using ModelData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.EntityFramework.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        internal RetroCollectNewContext _context;
        internal DbSet<T> dbSet;

        public GenericRepository(RetroCollectNewContext context)
        {
            _context = context;
            dbSet = context.Set<T>();
        }

        public virtual IEnumerable<T> Get(            
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            params Expression<Func<T, bool>>[] filter
            )
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                foreach (var item in filter)
                {
                    if (item != null) query = query.Where(item);
                }              
            }      

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual IEnumerable<string> GetDistinct(Func<T, string> query)
        {
            return dbSet.Select(query).Distinct().ToList();
        }

        public virtual T GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            T entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(T entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(T entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }
  
    }
}

