﻿using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Vilandagro.Core
{
    public interface IRepository : IDisposable
    {
        /// <summary>
        /// Get all entities
        /// </summary>
        /// <param name="typeToGet"></param>
        /// <returns></returns>
        IQueryable GetAll(Type typeToGet);

        /// <summary>
        /// Gets all entities
        /// </summary>        
        /// <returns>All entities</returns>
        IQueryable<T> GetAll<T>() where T : class;

        /// <summary>
        /// Gets all entities matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>All entities matching the predicate</returns>
        IQueryable<T> GetAll<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Set based on where condition
        /// </summary>
        /// <param name="predicate">The predicate</param>
        /// <returns>The records matching the given condition</returns>
        IQueryable<T> Where<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Find object by keys.
        /// </summary>
        /// <param name="keys">Specified the search keys.</param>
        /// <returns></returns>
        T Find<T>(params object[] keys) where T : class;

        /// <summary>
        /// Find object by keys in async manner
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keys"></param>
        /// <returns></returns>
        Task<T> FindAsync<T>(params object[] keys) where T : class;

        /// <summary>
        /// Determines if there are any entities matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>True if a match was found</returns>
        bool Any<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Determines if there are any entities matching the predicate in async manner
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Returns the first entity that matches the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>An entity matching the predicate</returns>
        T First<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Returns the first entity that matches the predicate in async manner
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>An entity matching the predicate</returns>
        Task<T> FirstAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Returns the first entity that matches the predicate else null
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>An entity matching the predicate else null</returns>
        T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Returns the first entity that matches the predicate else null in async mannger
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>An entity matching the predicate else null</returns>
        Task<T> FirstOrDefaultAsync<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Adds a given entity to the context
        /// </summary>
        /// <param name="entity">The entity to add to the context</param>
        void Add<T>(T entity) where T : class;

        /// <summary>
        /// Adds a given entities set to the context
        /// </summary>
        /// <param name="entities">The entities set to add to the context</param>
        void AddRange<T>(T[] entities) where T : class;

        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        bool DeleteById<T>(int id) where T : class;

        /// <summary>
        /// Deletes a given entity from the context
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        bool Delete<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Deletes a given entity from the context
        /// </summary>
        /// <param name="entities">The entities set to delete</param>
        void DeleteRange<T>(T[] entities) where T : class;

        /// <summary>
        /// Attaches a given entity to the context
        /// </summary>
        /// <param name="entity">The entity to attach</param>
        void Attach<T>(T entity) where T : class;

        /// <summary>
        /// Save changes
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Save changes in async mannger
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}