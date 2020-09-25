// <copyright file="ISaleRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface ISaleRepository : IGenericRepository<Sale>
    {
        Task<List<Branch>> GetAllBranchesAsync();

        Task<List<Client>> GetAllClientsAsync();

        Task<List<Employee>> GetAllSalesResponsiblesAsync();

        Task<List<WorkUnit>> GetAllWorkUnitsAsync();
    }
}