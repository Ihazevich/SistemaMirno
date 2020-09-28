// <copyright file="IDeliveryOrderRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="DeliveryOrder"/> model.
    /// </summary>
    public interface IDeliveryOrderRepository : IGenericRepository<DeliveryOrder>
    {
        /// <summary>
        /// Asynchronously retrieves all <see cref="Vehicle"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Vehicle>> GetAllVehiclesAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Employee"/> entities from the database
        /// that belong to the logistic department.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Employee>> GetAllLogisticResponsiblesAsync();

        /// <summary>
        /// Asynchronously retrieves all <see cref="Sale"/> entities from the database
        /// that haven't bee delivered yet.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<Sale>> GetAllNonDeliveredSalesAsync();
    }
}