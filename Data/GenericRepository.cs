using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data
{
    public abstract class GenericRepository<T> where T : class
    {
        protected DbContext _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }

        //pisemo genericke funkcije koje mozemo da primenimo nad svim repozitorijumima

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public bool Any(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Any(match); // izvrsicemo fju Any sa vim match komparatorom nad bazom pod
        }

        public virtual T GetById(object id)
        {
            return _context.Set<T>().Find(id);
        }

        public virtual T FirstOrDefault(Expression<Func<T, bool>> match, string includePropreties = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(includePropreties))
            {
                //da bude ovako: "Role, Subject"
                foreach (var prop in includePropreties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);  // formiramo upit
                }
            }

            return query.FirstOrDefault(match);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> match, string includePropreties = null) // includePropreties je opcioni parametar, npr ako ga ima prikazace uz usere i njihove role, a ako ga nema onda samo usere
        {
            var query = _context.Set<T>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(includePropreties))
            {
                //da bude ovako: "Role, Subject"
                foreach (var prop in includePropreties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);  // formiramo upit
                }
            }
            return query.Where(match);
        }

        public virtual T Add(T entity, string includePropreties = null)
        {
            var query = _context.Set<T>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(includePropreties))
            {
                //da bude ovako: "Role, Subject"
                foreach (var prop in includePropreties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);  // formiramo upit
                }
            }
            _context.Set<T>().Add(entity);

            // return query.Append(entity);
            // _context.SaveChanges();

            return entity;
        }

        public virtual void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            // _context.SaveChanges();

        }

        public virtual T Update(T entity, object key)
        {
            if (entity == null) return null;

            T exists = _context.Set<T>().Find(key);

            if (exists != null)
            {
                _context.Entry(exists).CurrentValues.SetValues(entity);
                // _context.SaveChanges();
            }

            return entity;
        }

        public long Count()
        {
            return _context.Set<T>().Count();
        }

        public long Count(Expression<Func<T, bool>> match)
        {
            return _context.Set<T>().Count(match);
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        // za brisanje contextra, diskoektovanje
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); //poziva garbice collector da se izbrise iz memorije
        }
    }
}
