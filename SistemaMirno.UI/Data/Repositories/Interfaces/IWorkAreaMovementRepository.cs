// <copyright file="IWorkAreaMovementRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="WorkAreaMovement"/> model.
    /// </summary>
    public interface IWorkAreaMovementRepository : IGenericRepository<WorkAreaMovement>
    {
        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkAreaMovement"/> entities from the database
        /// incoming to a specific <see cref="WorkArea"/> in a given date.
        /// </summary>
        /// <param name="id">The id of the work area.</param>
        /// <param name="date">The date.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkAreaMovement>> GetAllIncomingWorkAreaMovementsOfWorkAreaInDateAsync(int id, DateTime date);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkAreaMovement"/> entities from the database
        /// originating from a specific <see cref="WorkArea"/> in a given date.
        /// </summary>
        /// <param name="id">The id of the work area.</param>
        /// <param name="date">The date.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkAreaMovement>> GetAllOutgoingWorkAreaMovementsOfWorkAreaInDateAsync(int id, DateTime date);

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkArea"/> entities from the database
        /// that are related to a specific <see cref="Branch"/>.
        /// </summary>
        /// <param name="branchId">The branch id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkArea>> GetAllWorkAreasFromBranchAsync(int branchId);
    }
}