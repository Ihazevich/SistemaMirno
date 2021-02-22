// <copyright file="IRequisitionRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="Requisition"/> model.
    /// </summary>
    public interface IRequisitionRepository : IGenericRepository<Requisition>
    {
        /// <summary>
        /// Asynchronously retrieves all <see cref="Client"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Client>> GetAllClientsAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Product"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Product>> GetAllProductsAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Material"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Material>> GetAllMaterialsAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Color"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Color>> GetAllColorsAsync();

        /// <summary>
        /// Asynchronously retrieves the <see cref="WorkArea"/> entity thats marked as first from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<WorkArea> GetFirstWorkAreaAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="WorkUnit"/> entities that are not assigned from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<WorkUnit>> GetAllUnassignedWorkUnitsAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Requisition"/> entities that havent been fulfilled yet from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Requisition>> GetAllOpenRequisitionsAsync();
    }
}