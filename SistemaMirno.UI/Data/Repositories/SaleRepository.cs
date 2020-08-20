// <copyright file="SaleRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class SaleRepository : GenericRepository<Sale, MirnoDbContext>, ISaleRepository
    {
        public SaleRepository(MirnoDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Sale>> GetAllBetweenTwoDatesAsync(DateTime start, DateTime end)
        {
            return await Context.Sales.Where(s =>
                (s.Requisition.RequestedDate >= start && s.Requisition.RequestedDate <= end))
            .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Color>> GetColorsAsync()
        {
            return await Context.Colors.ToListAsync();
        }

        public async Task<IEnumerable<Material>> GetMaterialsAsync()
        {
            return await Context.Materials.ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsAsync()
        {
            return await Context.Clients.ToListAsync();
        }

        public async Task<int> GetRequisitionAreaIdAsync()
        {
            return (await Context.WorkAreas.SingleAsync(w => w.Name == "Pedidos")).Id;
        }
    }
}
