// <copyright file="ISaleRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="Role"/> model.
    /// </summary>
    public interface ISaleRepository : IGenericRepository<Sale>
    {
        /// <summary>
        /// Asynchronously retrieves all <see cref="Branch"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Branch>> GetAllBranchesAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Client"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Client>> GetAllClientsAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Employee"/> entities from the database that belong to the sales department.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Employee>> GetAllSalesResponsiblesAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkUnit"/> entities from the database that area available to sell.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkUnit>> GetAllWorkUnitsAsync();
    }
}