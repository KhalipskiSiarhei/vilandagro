using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Vilandagro.Infrastructure.EF
{
    public class Repository : IRepository
    {
        private DbContext _dbContext;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected DbContext DbContext
        {
            get { return _dbContext; }
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            // TODO: It is the same as GetAll/Find... Is it OK?
            return DbContext.Set<T>().AsEnumerable();
        }

        public IEnumerable<T> GetAll<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DbContext.Set<T>().Where(predicate).AsEnumerable();
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DbContext.Set<T>().Where(predicate).AsQueryable();
        }

        public IEnumerable<T> Find<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            // TODO: It is the same as GetAll/Find... Is it OK?
            return DbContext.Set<T>().Where(predicate).AsEnumerable();
        }

        public bool Any<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DbContext.Set<T>().Any(predicate);
        }

        public T First<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DbContext.Set<T>().Where(predicate).First();
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return DbContext.Set<T>().Where(predicate).FirstOrDefault();
        }

        public void Add<T>(T entity) where T : class
        {
            DbContext.Set<T>().Add(entity);
        }

        public void AddRange<T>(T[] entities) where T : class
        {
            DbContext.Set<T>().AddRange(entities);
        }

        public void Delete<T>(T entity) where T : class
        {
            DbContext.Set<T>().Remove(entity);
        }

        public void DeleteRange<T>(T[] entities) where T : class
        {
            DbContext.Set<T>().RemoveRange(entities);
        }

        public void Attach<T>(T entity) where T : class
        {
            DbContext.Set<T>().Attach(entity);
        }
    }
}