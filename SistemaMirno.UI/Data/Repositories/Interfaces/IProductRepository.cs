// <copyright file="IProductRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    /// <summary>
    /// Represents the interface of the data repository for the <see cref="Product"/> model.
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>
    {
        /// <summary>
        /// Asynchronously retrieves all <see cref="ProductCategory"/> entities from the database.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<List<ProductCategory>> GetAllProductCategoriesAsync();

        /// <summary>
        /// Asynchronously retrieves a <see cref="ProductCategory"/> entity with a matching name.
        /// </summary>
        /// <param name="productCategoryName">The name of the category.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<ProductCategory> GetProductCategoryByNameAsync(string productCategoryName);

        /// <summary>
        /// Asynchronously checks if the provided name aready exists in the database.
        /// </summary>
        /// <param name="productName">The name of the product.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task<bool> CheckForDuplicatesAsync(string productName);
    }
}