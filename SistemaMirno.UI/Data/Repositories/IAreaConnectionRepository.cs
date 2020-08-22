// <copyright file="IAreaConnectionRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// An interface for the area connection data repository.
    /// </summary>
    public interface IAreaConnectionRepository : IGenericRepository<WorkAreaConnection>
    {
        Task<IEnumerable<WorkAreaConnection>> GetByAreaIdAsync(int areaId);

        Task<IEnumerable<WorkArea>> GetWorkAreasAsync();

        Task<WorkArea> GetWorkAreaByIdAsync(int id);
    }
}