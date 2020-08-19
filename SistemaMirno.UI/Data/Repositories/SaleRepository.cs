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

            return await Context.Sales.Where(s =>
                (DateTime.Compare(s.Requisition.RequestedDate, start) >= 0 && DateTime.Compare(s.Requisition.RequestedDate, end.AddDays(1)) < 0)
            ).ToListAsync();
        }
    }
}
