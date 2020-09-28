// <copyright file="IWorkUnitRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="WorkUnit"/> model.
    /// </summary>
    public interface IWorkUnitRepository : IGenericRepository<WorkUnit>
    {
        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkUnit"/> entities from the database
        /// that are currently in a given work area.
        /// </summary>
        /// <param name="id">The id of the work area.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkUnit>> GetAllWorkUnitsCurrentlyInWorkAreaAsync(int id);

        /// <summary>
        /// Asynchronously retrieves a <see cref="WorkArea"/> entity from the database
        /// that matches the provided id.
        /// </summary>
        /// <param name="id">The id of the work area.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<WorkArea> GetWorkAreaByIdAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkUnit"/> entities from the database
        /// that are currently in process.
        /// </summary>
        /// <param name="id">The id of the work area.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkUnit>> GetWorkUnitsInProcessAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkArea"/> entities from the database
        /// that report if they have work units in process.
        /// </summary>
        /// <param name="id">The id of the work area.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkArea>> GetWorkAreasThatReportInProcessAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkAreaConnection"/> entities from the database
        /// that originate from a given work area.
        /// </summary>
        /// <param name="workAreaId">The id of the work area.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkAreaConnection>> GetWorkAreaOutgoingConnectionsAsync(int workAreaId);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkUnit"/> entities from the database
        /// that are currently in the last(Stock) work area.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkUnit>> GetAllWorkUnitsInAllLastWorkAreasAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Branch"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Branch>> GetAllBranchesAsync();

        /// <summary>
        /// Asynchronously retrieves a <see cref="WorkArea"/> entity from the database
        /// that belongs to a specific branch and is marked as last.
        /// </summary>
        /// <param name="id">The id of the branch.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<WorkArea> GetLastWorkAreaFromBranchIdAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkArea"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkArea>> GetAllWorkAreasAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Material"/> entities from the database
        /// that are currently in the last(Stock) work area.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Material>> GetAllMaterialsAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Color"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Color>> GetAllColorsAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Product"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Product>> GetAllProductsAsync();
    }
}