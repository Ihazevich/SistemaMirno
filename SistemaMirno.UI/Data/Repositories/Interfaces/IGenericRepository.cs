// <copyright file="IGenericRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface for a generic data repository.
    /// </summary>
    /// <typeparam name="T">The entity type of the repository.</typeparam>
    public interface IGenericRepository<T>
        where T : ModelBase
    {
        /// <summary>
        /// Asynchronously adds an entity to the database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> AddAsync(T entity);

        /// <summary>
        /// Asynchronously adds a list of entities to the database.
        /// </summary>
        /// <param name="entites">The list of entities.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> AddRangeAsync(IList<T> entites);

        /// <summary>
        /// Asynchronously saves an entity to the database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> SaveAsync(T entity);

        /// <summary>
        /// Asynchronously deletes an entity with a given id and timestamp from the database.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <param name="timeStamp">The timestamp of the entity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> DeleteAsync(int id, byte[] timeStamp);

        /// <summary>
        /// Asynchronously deletes an entity from the database.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> DeleteAsync(T entity);

        /// <summary>
        /// Asynchronously gets an entity by id from the database.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<T> GetByIdAsync(int? id);

        /// <summary>
        /// Gets all entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Asks whether the database context currently has changes.
        /// </summary>
        /// <returns>The state of the database change tracker.</returns>
        bool HasChanges();
    }
}