// <copyright file="IEmployeeRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// An interface for the employee data repository.
    /// </summary>
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
    }
}