// <copyright file="IWorkAreaRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IWorkAreaRepository : IGenericRepository<WorkArea>
    {
        Task<List<WorkAreaConnection>> GetAllWorkAreaConnectionsFromWorkAreaAsync(int id);

        Task<List<Branch>> GetAllBranchesAsync();

        Task<List<WorkArea>> GetAllWorkAreasAsync();

        Task<List<Role>> GetAllRolesAsync();

        Task<bool> CheckIfLastExistsAsync(int id);

        Task<bool> CheckIfFirstExistsAsync(int id);

        Task<List<WorkArea>> GetAllWorkAreasFromBranchAsync(int id);
    }
}