// <copyright file="IWorkAreaRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="WorkArea"/> model.
    /// </summary>
    public interface IWorkAreaRepository : IGenericRepository<WorkArea>
    {
        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkAreaConnection"/> entities from the database
        /// related to a specific <see cref="WorkArea"/>.
        /// </summary>
        /// <param name="id">The id of the work area.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkAreaConnection>> GetAllWorkAreaConnectionsFromWorkAreaAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all <see cref="Branch"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Branch>> GetAllBranchesAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkArea"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkArea>> GetAllWorkAreasAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Role"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Role>> GetAllRolesAsync();

        /// <summary>
        /// Asynchronously checks if a last <see cref="WorkArea"/> entity exists in the database.
        /// </summary>
        /// <param name="id">The id of the current branch.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> CheckIfLastExistsAsync(int id);

        /// <summary>
        /// Asynchronously checks if a first <see cref="WorkArea"/> entity exists in the database.
        /// </summary>
        /// <param name="id">The id of the current branch.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> CheckIfFirstExistsAsync(int id);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkArea"/> entities from the database
        /// that are realted to a specific <see cref="Branch"/>.
        /// </summary>
        /// <param name="id">The id of the branch.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkArea>> GetAllWorkAreasFromBranchAsync(int id);
    }
}