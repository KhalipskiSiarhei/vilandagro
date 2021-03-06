﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vilandagro.Core;
using Vilandagro.Core.Entities;

namespace Vilandagro.Infrastructure.EF
{
    public class Repository : IRepository
    {
        private IRequestAware _requestAware = null;

        private DbContext _dbContext = null;

        public Repository(IRequestAware requestAware)
        {
            _requestAware = requestAware;
        }

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public DbContext DbContext
        {
            get
            {
                if (_requestAware == null)
                {
                    return _dbContext;
                }
                else
                {
                    return (DbContext)_requestAware[DbContextManager.DbContextKey];
                }
            }
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

        public async Task<T> FindAsync<T>(params object[] keys) where T : class
        {
            return await DbContext.Set<T>().FindAsync(keys);
        }

        public bool Any<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().Any(predicate);
        }

        public async Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await GetAll<T>().AnyAsync(predicate);
        }

        public T First<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().Where(predicate).First();
        }

        public async Task<T> FirstAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await GetAll<T>().Where(predicate).FirstAsync();
        }

        public T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return GetAll<T>().Where(predicate).FirstOrDefault();
        }

        public async Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class
        {
            return await GetAll<T>().Where(predicate).FirstOrDefaultAsync();
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

            var id = GetEntityId(entity);

            if (id == 0)
            {
                DbContext.Entry(entity).State = EntityState.Added;
            }
            else if (id > 0)
            {
                DbContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public int SaveChanges()
        {
            foreach (var dbEntity in DbContext.ChangeTracker.Entries().Where(x => x.State == EntityState.Added || x.State == EntityState.Modified))
            {
                if (dbEntity.State == EntityState.Added)
                {
                    var entity = dbEntity.Entity as IVersion;
                    if (entity != null)
                    {
                        var iVersion = entity;
                        iVersion.Version = 1;
                    }
                    else
                    {
                        var version = GetVersionProperty(dbEntity);

                        if (version != null)
                        {
                            version.CurrentValue = 1;
                        }
                    }
                }
                else if (dbEntity.State == EntityState.Modified)
                {
                    var entity = dbEntity.Entity as IVersion;
                    if (entity != null)
                    {
                        var iVersion = entity;
                        iVersion.Version += 1;
                    }
                    else
                    {
                        var version = GetVersionProperty(dbEntity);

                        if (version != null)
                        {
                            version.CurrentValue = (int)version.CurrentValue + 1;
                        }
                    }
                }
            }

            return DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await DbContext.SaveChangesAsync();
        }

        protected internal int GetEntityId<T>(T entity) where T : class
        {
            var iEntity = entity as IEntity;
            if (iEntity != null)
            {
                return iEntity.Id;
            }
            else
            {
                var idProperty = (typeof (T).GetProperty("Id"));

                if (idProperty != null)
                {
                    var idValue = idProperty.GetValue(entity);

                    if (idValue is int)
                    {
                        return (int) idValue;
                    }

                    throw new NotSupportedException(
                        string.Format(
                            "Entity of type={0} has Id property with type={1}. This type does not supported, {2} should be used only",
                            typeof (T), idProperty.MemberType, typeof (int)));
                }
                return -1;
            }
        }

        private DbPropertyEntry GetVersionProperty(DbEntityEntry dbEntry)
        {
            return dbEntry.Property("Version");
        }

        #region IDisposable
        ~Repository() 
        {
            // Finalizer calls Dispose(false)
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dbContext != null)
                {
                    _dbContext.Dispose();
                    _dbContext = null;
                }                
            }
        }

        #endregion
    }
}