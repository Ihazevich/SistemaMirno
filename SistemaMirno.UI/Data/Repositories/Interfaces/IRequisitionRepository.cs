﻿using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

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