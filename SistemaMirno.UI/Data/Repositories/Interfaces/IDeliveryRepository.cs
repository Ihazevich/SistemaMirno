// <copyright file="IDeliveryRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="Delivery"/> model.
    /// </summary>
    public interface IDeliveryRepository : IGenericRepository<Delivery>
    {
        /// <summary>
        /// Asynchronously retrieves all <see cref="Delivery"/> entities from the database
        /// that are in process.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<IEnumerable<Delivery>> GetAllInProcessAsync();
    }
}