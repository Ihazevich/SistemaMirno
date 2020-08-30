﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;

namespace SistemaMirno.UI.Data.Repositories.Interfaces
{
    public interface IWorkAreaMovementRepository : IGenericRepository<WorkAreaMovement>
    {
        Task<List<WorkAreaMovement>> GetAllIncomingWorkAreaMovementsOfWorkAreaInDateAsync(int id, DateTime date);

        Task<List<WorkAreaMovement>> GetAllOutgoingWorkAreaMovementsOfWorkAreaInDateAsync(int id, DateTime date);

        Task<List<WorkArea>> GetAllWorkAreasFromBranchAsync(int branchId);
    }
}