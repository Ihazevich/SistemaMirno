// <copyright file="IRequisitionRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IRequisitionRepository : IGenericRepository<Requisition>
    {
        Task<List<Client>> GetAllClientsAsync();

        Task<List<Product>> GetAllProductsAsync();

        Task<List<Material>> GetAllMaterialsAsync();

        Task<List<Color>> GetAllColorsAsync();

        Task<WorkArea> GetFirstWorkAreaAsync();

        Task<List<WorkUnit>> GetAllUnassignedWorkUnitsAsync();
    }
}