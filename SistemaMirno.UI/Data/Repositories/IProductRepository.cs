// <copyright file="IProductRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// An interface for the product data repository.
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>
    {
    }
}