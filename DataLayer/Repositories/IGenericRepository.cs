using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void Delete(object id);
        void Delete(T entityToDelete);
        IEnumerable<T> Get(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, params Expression<Func<T, bool>>[] filter);
        T GetByID(object id);
        IEnumerable<string> GetDistinct(Func<T, string> query);
        void Insert(T entity);
        void Update(T entityToUpdate);
    }
}