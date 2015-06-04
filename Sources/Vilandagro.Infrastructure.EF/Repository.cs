using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vilandagro.Core;

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

        public IQueryable GetAll(Type typeToGet)
        {
            return DbContext.Set(typeToGet).AsQueryable();
        }

        public IQueryable<T> GetAll<T>() where T : class
        {
            return DbContext.Set<T>().AsQueryable();
        }

        public IQueryable<T> GetAll<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().Where(predicate).AsQueryable();
        }

        public IQueryable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().Where(predicate).AsQueryable();
        }

        public T Find<T>(params object[] keys) where T : class
        {
            return DbContext.Set<T>().Find(keys);
        }

        public Task<T> FindAsync<T>(params object[] keys) where T : class
        {
            return DbContext.Set<T>().FindAsync(keys);
        }

        public bool Any<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().Any(predicate);
        }

        public Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().AnyAsync(predicate);
        }

        public T First<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().Where(predicate).First();
        }

        public Task<T> FirstAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().Where(predicate).FirstAsync();
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().Where(predicate).FirstOrDefault();
        }

        public Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().Where(predicate).FirstOrDefaultAsync();
        }

        public void Add<T>(T entity) where T : class
        {
            DbContext.Set<T>().Add(entity);
        }

        public void AddRange<T>(T[] entities) where T : class
        {
            DbContext.Set<T>().AddRange(entities);
        }

        public bool DeleteById<T>(int id) where T : class
        {
            var entity = Find<T>(id);

            if (entity != null)
            {
                Delete(entity);
            }
            return entity != null;
        }

        public void Delete<T>(T entity) where T : class
        {
            DbContext.Set<T>().Remove(entity);
        }

        public bool Delete<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            var entities = Where(predicate).ToList();
            var isEntitiesToRemove = entities.Any();

            if (isEntitiesToRemove)
            {
                DbContext.Set<T>().RemoveRange(entities);
            }
            return isEntitiesToRemove;
        }

        public void DeleteRange<T>(T[] entities) where T : class
        {
            DbContext.Set<T>().RemoveRange(entities);
        }

        public void Attach<T>(T entity) where T : class
        {
            DbContext.Set<T>().Attach(entity);
        }

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }
    }
}