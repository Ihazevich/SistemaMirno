// <copyright file="IProductRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    /// <summary>
    /// An interface for the product data repository.
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>
    {
        /// <summary>
        /// Returns the id of a product category matching with the provided name.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<int> GetProductCategoryIdAsync(string categoryName);

        Task<bool> CheckForDuplicatesAsync(string name);
    }
}