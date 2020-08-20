// <copyright file="ISaleRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public interface ISaleRepository : IGenericRepository<Sale>
    {
        Task<IEnumerable<Sale>> GetAllBetweenTwoDatesAsync(DateTime start, DateTime end);

        Task<IEnumerable<Product>> GetProductsAsync();

        Task<IEnumerable<Color>> GetColorsAsync();

        Task<IEnumerable<Material>> GetMaterialsAsync();

        Task<IEnumerable<Client>> GetClientsAsync();

        Task<int> GetRequisitionAreaIdAsync();
    }
}
