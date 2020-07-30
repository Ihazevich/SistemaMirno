﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// Interface for the <see cref="ProductionAreaRepository"/> class.
    /// </summary>
    public interface IProductionAreaRepository
    {
        /// <summary>
        /// Gets all the Production Areas from the database context.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        Task<List<ProductionArea>> GetAllAsync();

        /// <summary>
        /// Gets a Production Area that matches the provided id.
        /// </summary>
        /// <param name="id">A <see cref="int"/> value representing the id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<ProductionArea> GetByIdAsync(int id);

        /// <summary>
        /// Saves all changes made to the database context.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        Task SaveAsync();
    }
}