// <copyright file="IClientRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="Client"/> model.
    /// </summary>
    public interface IClientRepository : IGenericRepository<Client>
    {
    }
}