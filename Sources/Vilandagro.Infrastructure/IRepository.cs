using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Vilandagro.Infrastructure
{
    public interface IRepository
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
        /// Finds an entity matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>An entity matching the predicate</returns>
        IQueryable<T> Find<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Determines if there are any entities matching the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>True if a match was found</returns>
        bool Any<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Returns the first entity that matches the predicate
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>An entity matching the predicate</returns>
        T First<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Returns the first entity that matches the predicate else null
        /// </summary>
        /// <param name="predicate">The filter clause</param>
        /// <returns>An entity matching the predicate else null</returns>
        T FirstOrDefault<T>(Expression<Func<T, bool>> predicate) where T : class;

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
        /// Deletes a given entity from the context
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        void Delete<T>(T entity) where T : class;

        /// <summary>
        /// Delete objects from database by specified filter expression.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        void Delete<T>(Expression<Func<T, bool>> predicate) where T : class;

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
        void SaveChanges();
    }
}