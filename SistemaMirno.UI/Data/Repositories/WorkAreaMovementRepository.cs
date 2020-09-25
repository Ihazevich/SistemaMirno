﻿// <copyright file="WorkAreaMovementRepository.cs" company="HazeLabs">
// Copyright (c) HazeLabs. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Prism.Events;
using SistemaMirno.DataAccess;
using SistemaMirno.Model;
using SistemaMirno.UI.Data.Repositories.Interfaces;
using SistemaMirno.UI.Event;

namespace SistemaMirno.UI.Data.Repositories
{
    public class WorkAreaMovementRepository : GenericRepository<WorkAreaMovement, MirnoDbContext>, IWorkAreaMovementRepository
    {
        public WorkAreaMovementRepository(Func<MirnoDbContext> contextCreator, IEventAggregator eventAggregator)
            : base(contextCreator, eventAggregator)
        {
        }

        public async Task<List<WorkAreaMovement>> GetAllIncomingWorkAreaMovementsOfWorkAreaInDateAsync(
            int id,
            DateTime date)
        {
            try
            {
                return await Context.WorkAreaMovements.Where(m => m.ToWorkAreaId == id
                                                                  && m.Date.Year == date.Year &&
                                                                  m.Date.Month == date.Month &&
                                                                  m.Date.Day == date.Day).ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        public async Task<List<WorkAreaMovement>> GetAllOutgoingWorkAreaMovementsOfWorkAreaInDateAsync(
            int id,
            DateTime date)
        {
            try
            {
                return await Context.WorkAreaMovements.Where(m => m.FromWorkAreaId == id
                                                                  && m.Date.Year == date.Year &&
                                                                  m.Date.Month == date.Month &&
                                                                  m.Date.Day == date.Day).ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }

        public async Task<List<WorkArea>> GetAllWorkAreasFromBranchAsync(int branchId)
        {
            try
            {
                return await Context.WorkAreas.Where(w => w.BranchId == branchId).ToListAsync();
            }
            catch (Exception e)
            {
                EventAggregator.GetEvent<ShowDialogEvent>().Publish(new ShowDialogEventArgs
                {
                    Message = $"Error inesperado [{e.Message}] contacte al Administrador del Sistema",
                    Title = "Error",
                });
                return null;
            }
        }
    }
}
