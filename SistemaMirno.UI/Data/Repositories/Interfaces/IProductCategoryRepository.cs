// <copyright file="IProductCategoryRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="ProductCategory"/> model.
    /// </summary>
    public interface IProductCategoryRepository : IGenericRepository<ProductCategory>
    {
    }
}