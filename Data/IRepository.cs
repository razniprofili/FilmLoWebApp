using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data
{
   public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        bool Any(Expression<Func<T, bool>> match);
        T GetById(object id);
        T FirstOrDefault(Expression<Func<T, bool>> match, string includePropreties = null);
        IQueryable<T> Find(Expression<Func<T, bool>> match, string includePropreties = null);
        T Add(T entity, string includePropreties = null);
        void Delete(T entity);
        T Update(T entity, object key);
        long Count();
        long Count(Expression<Func<T, bool>> match);
        void Save();
        void Dispose();

    }
}
