// <copyright file="IVehicleRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="Vehicle"/> model.
    /// </summary>
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
    }
}