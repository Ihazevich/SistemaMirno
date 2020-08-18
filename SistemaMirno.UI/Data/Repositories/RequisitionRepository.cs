// <copyright file="RequisitionRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using SistemaMirno.DataAccess;
using SistemaMirno.Model;

namespace SistemaMirno.UI.Data.Repositories
{
    public class RequisitionRepository : GenericRepository<Requisition, MirnoDbContext>, IRequisitionRepository
    {
        public RequisitionRepository(MirnoDbContext context)
            : base(context)
        {
        }
    }
}
