// <copyright file="IWorkUnitRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IWorkUnitRepository : IGenericRepository<WorkUnit>
    {
        Task<List<WorkUnit>> GetAllWorkUnitsCurrentlyInWorkAreaAsync(int id);

        Task<WorkArea> GetWorkAreaById(int id);

        Task<List<WorkUnit>> GetWorkUnitsInProcessAsync();

        Task<List<WorkArea>> GetWorkAreasThatReportInProcess();

        Task<List<WorkAreaConnection>> GetWorkAreaOutgoingConnections(int workAreaId);

        Task<List<WorkUnit>> GetAllWorkUnitsInAllLastWorkAreasAsync();

        Task<List<Branch>> GetAllBranchesAsync();

        Task<WorkArea> GetLastWorkAreaFromBranchIdAsync(int id);

        Task<List<WorkArea>> GetAllWorkAreasAsync();

        Task<List<Material>> GetAllMaterialsAsync();

        Task<List<Color>> GetAllColorsAsync();

        Task<List<Product>> GetAllProductsAsync();
    }
}