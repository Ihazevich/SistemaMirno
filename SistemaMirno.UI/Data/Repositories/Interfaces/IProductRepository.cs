// <copyright file="IProductRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<List<ProductCategory>> GetAllProductCategoriesAsync();

        Task<ProductCategory> GetProductCategoryByNameAsync(string productCategoryName);

        Task<bool> CheckForDuplicatesAsync(string productName);
    }
}