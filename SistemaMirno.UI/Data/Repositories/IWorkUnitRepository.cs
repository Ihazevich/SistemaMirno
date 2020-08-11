// <copyright file="IWorkUnitRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// An interface for the work unit data repository.
    /// </summary>
    public interface IWorkUnitRepository : IGenericRepository<WorkUnit>
    {
        /// <summary>
        /// Gets all the Work Units that match the provided Work Area id.
        /// </summary>
        /// <param name="id">A <see cref="int"/> value representing the id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<WorkUnit>> GetByAreaIdAsync(int areaId);

        Task<WorkArea> GetWorkAreaByIdAsync(int areaId);
    }
}