// <copyright file="SaleRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

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
    }
}
